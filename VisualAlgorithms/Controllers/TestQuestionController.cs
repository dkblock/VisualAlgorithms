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
    public class TestQuestionController : Controller
    {
        private readonly ApplicationContext _db;

        public TestQuestionController(ApplicationContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create(int testId, bool isNewTest)
        {
            var testQuestion = new TestQuestion {TestId = testId};
            var testAnswers = new List<TestAnswer>();

            for (var i = 0; i < 10; i++)
                testAnswers.Add(new TestAnswer());

            var questionModel = new TestQuestionViewModel
            {
                TestQuestion = testQuestion,
                TestAnswers = testAnswers,
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
            var answersCount = testAnswers.Count;

            for (var i = 0; i < 10 - answersCount; i++)
                testAnswers.Add(new TestAnswer());

            var questionModel = new TestQuestionViewModel
            {
                TestQuestion = testQuestion,
                TestAnswers = testAnswers,
                Image = testQuestion.Image
            };

            questionModel.TestQuestion.TestAnswers = new List<TestAnswer>();

            return View(questionModel);
        }
    }
}