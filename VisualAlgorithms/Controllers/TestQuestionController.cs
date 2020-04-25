using System.Collections.Generic;
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
        public IActionResult Create(int testId)
        {
            var testQuestion = new TestQuestion {TestId = testId};
            var testAnswers = new List<TestAnswer>();

            for (var i = 0; i < 10; i++)
                testAnswers.Add(new TestAnswer());

            var questionCreateModel = new TestQuestionViewModel
            {
                TestQuestion = testQuestion,
                TestAnswers = testAnswers
            };

            return View(questionCreateModel);
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var testQuestion = await _db.TestQuestions
                .Include(tq => tq.TestAnswers)
                .SingleOrDefaultAsync(tq => tq.Id == id);

            if (testQuestion == null)
                return RedirectToAction("Tests", "Admin");

            var questionModel = new TestQuestionViewModel
            {
                TestQuestion = testQuestion,
                TestAnswers = testQuestion.TestAnswers,
                Image = testQuestion.Image
            };

            return View(questionModel);
        }
    }
}