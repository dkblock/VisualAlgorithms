using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQuestionController : Controller
    {
        private readonly ApplicationContext _db;

        public TestQuestionController(ApplicationContext db)
        {
            _db = db;

            ViewBag.SelectAnswer = TestQuestionType.SelectAnswer;
            ViewBag.FreeAnswer = TestQuestionType.FreeAnswer;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create(int testId)
        {
            var testQuestion = new TestQuestion { TestId = testId };
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
            var testQuestionId = await AddQuestion(questionModel.TestQuestion);
            await AddQuestionAnswers(testQuestionId, questionModel.TestAnswers);

            if (questionModel.TestQuestion.IsLastQuestion)
                return true;

            return false;
        }

        private async Task<int> AddQuestion(TestQuestion testQuestion)
        {
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
