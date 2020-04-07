using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    public class TestQuestionController : Controller
    {
        private readonly ApplicationContext _db;

        public TestQuestionController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Create(int testId)
        {
            var testQuestion = new TestQuestion { TestId = testId };
            return View(testQuestion);
        }

        [HttpPost]
        public async Task<IActionResult> OnNextQuestionAddition(TestQuestion testQuestionModel)
        {
            await AddQuestion(testQuestionModel);
            return RedirectToAction("Create", new { testId = testQuestionModel.TestId });
        }

        [HttpPost]
        public async Task<IActionResult> OnEndTestCreation(TestQuestion testQuestionModel)
        {
            await AddQuestion(testQuestionModel);
            return RedirectToAction("Tests", "Admin");
        }

        private async Task AddQuestion(TestQuestion testQuestionModel)
        {
            if (ModelState.IsValid)
            {
                var testQuestion = new TestQuestion
                {
                    Question = testQuestionModel.Question,
                    Answer = testQuestionModel.Answer,
                    TestId = testQuestionModel.TestId
                };

                await _db.TestQuestions.AddAsync(testQuestion);
                await _db.SaveChangesAsync();
            }
        }
    }
}
