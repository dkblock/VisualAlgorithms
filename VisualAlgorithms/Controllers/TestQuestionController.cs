using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQuestionController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IWebHostEnvironment _env;

        public TestQuestionController(ApplicationContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create(int testId)
        {
            var testQuestion = new TestQuestion {TestId = testId};
            var testAnswers = new List<TestAnswer>();

            for (int i = 0; i < 10; i++)
                testAnswers.Add(new TestAnswer());

            var questionCreateModel = new TestQuestionCreateViewModel
            {
                TestQuestion = testQuestion,
                TestAnswers = testAnswers
            };

            return View(questionCreateModel);
        }

        [HttpPost]
        [Route("next")]
        public async Task<bool> OnNextQuestionCreation(TestQuestionCreateViewModel questionModel)
        {
            var testQuestionId = await AddQuestion(questionModel.TestQuestion, questionModel.Image);
            await AddQuestionAnswers(testQuestionId, questionModel.TestAnswers);

            if (questionModel.TestQuestion.IsLastQuestion)
                return true;

            return false;
        }

        [HttpPost]
        [Route("uploadImg")]
        public async Task<string> OnQuestionImageUploading()
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
        public void ClearQuestionImage([FromForm] string fileName)
        {
            var path = Path.Combine(_env.WebRootPath, "images", "test-questions", fileName);
            System.IO.File.Delete(path);
        }

        private async Task<int> AddQuestion(TestQuestion testQuestion, string image)
        {
            testQuestion.Image = image;
            var result = await _db.TestQuestions.AddAsync(testQuestion);
            await _db.SaveChangesAsync();

            return result.Entity.Id;
        }

        private async Task AddQuestionAnswers(int questionId, List<TestAnswer> testAnswers)
        {
            testAnswers.RemoveAll(a => a.Answer == null);

            foreach (var answer in testAnswers)
                answer.TestQuestionId = questionId;

            var result = await _db.TestAnswers.AddAsync(testAnswers.First());
            await _db.SaveChangesAsync();

            var correctAnswerId = result.Entity.Id;
            var testQuestion = await _db.TestQuestions.FindAsync(questionId);
            testQuestion.CorrectAnswerId = correctAnswerId;
            _db.Entry(testQuestion).State = EntityState.Modified;

            if (testAnswers.Count > 1)
            {
                for (int i = 1; i < testAnswers.Count; i++)
                    await _db.TestAnswers.AddAsync(testAnswers[i]);
            }

            await _db.SaveChangesAsync();
        }
    }
}