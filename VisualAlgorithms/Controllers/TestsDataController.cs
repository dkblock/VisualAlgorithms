using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.AppMiddleware;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsDataController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly AccessManager _accessManager;

        public TestsDataController(ApplicationContext db, IWebHostEnvironment env, AccessManager accessManager)
        {
            _db = db;
            _env = env;
            _accessManager = accessManager;
        }

        [HttpPost]
        [Route("createQuestion")]
        public async Task<bool> CreateTestQuestion(TestQuestionViewModel questionModel)
        {
            if (!await _accessManager.HasAdminAccess(questionModel.UserId))
                return false;

            var testQuestionId = await AddTestQuestion(questionModel);
            var correctAnswerId = await AddTestQuestionAnswers(testQuestionId, questionModel.TestAnswers);
            var testQuestion = await _db.TestQuestions.FindAsync(testQuestionId);

            testQuestion.CorrectAnswerId = correctAnswerId;
            _db.Entry(testQuestion).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return questionModel.TestQuestion.IsLastQuestion;
        }

        private async Task<int> AddTestQuestion(TestQuestionViewModel questionModel)
        {
            var testQuestion = questionModel.TestQuestion;
            testQuestion.Image = questionModel.Image;

            if (!questionModel.IsNewTest)
            {
                await ResetLastQuestion(questionModel.TestQuestion.TestId);
                testQuestion.IsLastQuestion = true;
            }

            var result = await _db.TestQuestions.AddAsync(testQuestion);
            await _db.SaveChangesAsync();

            return result.Entity.Id;
        }

        private async Task<int> AddTestQuestionAnswers(int questionId, List<TestAnswer> testAnswers)
        {
            testAnswers.RemoveAll(ta => ta.Answer == null);

            foreach (var answer in testAnswers)
                answer.TestQuestionId = questionId;

            var result = await _db.TestAnswers.AddAsync(testAnswers.First());
            await _db.SaveChangesAsync();
            var correctAnswerId = result.Entity.Id;

            if (testAnswers.Count > 1)
            {
                for (var i = 1; i < testAnswers.Count; i++)
                    await _db.TestAnswers.AddAsync(testAnswers[i]);
            }

            await _db.SaveChangesAsync();

            return correctAnswerId;
        }

        private async Task ResetLastQuestion(int testId)
        {
            var testQuestions = await _db.TestQuestions
                .Where(tq => tq.TestId == testId)
                .ToListAsync();

            var lastQuestion = testQuestions.Single(tq => tq.IsLastQuestion);
            lastQuestion.IsLastQuestion = false;
            _db.Entry(lastQuestion).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        [HttpPost]
        [Route("editQuestion")]
        public async Task<bool> EditTestQuestion(TestQuestionViewModel questionModel)
        {
            if (!await _accessManager.HasAdminAccess(questionModel.UserId))
                return false;

            await EditTestQuestionAnswers(questionModel.TestQuestion.Id, questionModel.TestAnswers);
            await EditTestQuestion(questionModel.TestQuestion, questionModel.Image);

            var testQuestions = await _db.TestQuestions
                .Where(tq => tq.TestId == questionModel.TestQuestion.TestId)
                .ToListAsync();

            var lastQuestion = testQuestions.Last();
            lastQuestion.IsLastQuestion = true;
            _db.Entry(lastQuestion).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return true;
        }

        private async Task EditTestQuestion(TestQuestion newQuestion, string image)
        {
            var testQuestion = await _db.TestQuestions.FindAsync(newQuestion.Id);
            testQuestion.Question = newQuestion.Question;
            testQuestion.TestQuestionType = newQuestion.TestQuestionType;
            testQuestion.Image = image;
            testQuestion.CorrectAnswerId = newQuestion.CorrectAnswerId;
            testQuestion.IsLastQuestion = false;

            _db.Entry(testQuestion).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private async Task EditTestQuestionAnswers(int questionId, List<TestAnswer> newAnswers)
        {
            newAnswers.RemoveAll(ta => ta.Answer == null);
            var testAnswers = await _db.TestAnswers
                .Where(ta => ta.TestQuestionId == questionId)
                .ToListAsync();

            var answersToUpdate = newAnswers.Select(na => na.Id).Intersect(testAnswers.Select(ta => ta.Id)).ToList();
            var answersToAdd = newAnswers.Where(na => na.Id == 0).ToList();
            var answersToRemove = testAnswers.Select(na => na.Id).Except(newAnswers.Select(ta => ta.Id)).ToList();

            await UpdateTestQuestionAnswers(newAnswers, answersToUpdate);

            if (answersToAdd.Any())
                await AddTestQuestionAnswers(questionId, answersToAdd);

            if (answersToRemove.Any())
                await RemoveTestQuestionAnswers(answersToRemove);
        }

        private async Task UpdateTestQuestionAnswers(List<TestAnswer> newAnswers, IEnumerable<int> answersToUpdate)
        {
            foreach (var answerId in answersToUpdate)
            {
                var answer = await _db.TestAnswers.FindAsync(answerId);
                answer.Answer = newAnswers.Single(na => na.Id == answerId).Answer;
                _db.Entry(answer).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
        }

        private async Task RemoveTestQuestionAnswers(IEnumerable<int> answersToRemove)
        {
            foreach (var answerId in answersToRemove)
            {
                var answer = await _db.TestAnswers.FindAsync(answerId);
                _db.TestAnswers.Remove(answer);
            }

            await _db.SaveChangesAsync();
        }

        [HttpPost]
        [Route("uploadImg")]
        public async Task<string> UploadTestQuestionImage()
        {
            var file = HttpContext.Request.Form.Files.FirstOrDefault();

            if (file != null)
            {
                var ext = Path.GetExtension(file.Name);
                var fileName = $"{Guid.NewGuid().ToString()}{ext}";
                var path = Path.Combine(_env.WebRootPath, "images", "test-questions", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }

            return null;
        }

        [HttpPost]
        [Route("clearImg")]
        public void ClearTestQuestionImage([FromForm] string fileName)
        {
            var path = Path.Combine(_env.WebRootPath, "images", "test-questions", fileName);
            System.IO.File.Delete(path);
        }
    }
}