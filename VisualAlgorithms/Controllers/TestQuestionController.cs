using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
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
        public async Task<IActionResult> Create(int testId, bool isNewTest)
        {
            var testQuestion = new TestQuestion
            {
                TestId = testId,
                TestQuestionType = TestQuestionType.FreeAnswer
            };
            var testAnswers = new List<TestAnswer> {new TestAnswer()};

            var questionModel = new TestQuestionViewModel
            {
                TestQuestion = testQuestion,
                TestAnswers = testAnswers,
                QuestionNumber = await GetQuestionNumber(testId),
                IsNewTest = isNewTest
            };

            return View(questionModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var testQuestion = await _db.TestQuestions.FindAsync(id);

            if (testQuestion == null)
                return RedirectToAction("Tests", "Admin");

            var testAnswers = await _db.TestAnswers
                .Where(ta => ta.TestQuestionId == id)
                .ToListAsync();

            var questionModel = new TestQuestionViewModel
            {
                TestQuestion = testQuestion,
                TestAnswers = testAnswers,
                QuestionNumber = await GetQuestionNumber(testQuestion.TestId, testQuestion.Id),
                Image = testQuestion.Image
            };

            questionModel.TestQuestion.TestAnswers = new List<TestAnswer>();

            return View(questionModel);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var testQuestion = await _db.TestQuestions.FindAsync(id);
            return View(testQuestion);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(TestQuestion testQuestion)
        {
            if (!string.IsNullOrEmpty(testQuestion.Image))
            {
                var path = Path.Combine(_env.WebRootPath, "images", "test-questions", testQuestion.Image);
                System.IO.File.Delete(path);
            }

            _db.TestQuestions.Remove(testQuestion);
            await _db.SaveChangesAsync();

            if (testQuestion.IsLastQuestion)
            {
                var testQuestions = await _db.TestQuestions
                    .Where(tq => tq.TestId == testQuestion.TestId)
                    .ToListAsync();

                var lastQuestion = testQuestions.Last();
                lastQuestion.IsLastQuestion = true;
                _db.Entry(lastQuestion).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Edit", "Tests", new {id = testQuestion.TestId});
        }

        private async Task<int> GetQuestionNumber(int testId, int questionId = -1)
        {
            var testQuestions = await _db.TestQuestions
                .Where(tq => tq.TestId == testId)
                .ToListAsync();

            if (questionId == -1)
                return testQuestions.Count + 1;

            return testQuestions.FindIndex(tq => tq.Id == questionId) + 1;
        }
    }
}