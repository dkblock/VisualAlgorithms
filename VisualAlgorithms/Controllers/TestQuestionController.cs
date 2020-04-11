using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    [Authorize(Roles = "admin")]
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
        public async Task<IActionResult> OnNextQuestionCreation(TestQuestion testQuestion)
        {
            await AddQuestion(testQuestion);
            return RedirectToAction("Create", new { testId = testQuestion.TestId });
        }

        [HttpPost]
        public async Task<IActionResult> OnEndTestCreation(TestQuestion testQuestion)
        {
            testQuestion.IsLastQuestion = true;
            await AddQuestion(testQuestion);

            return RedirectToAction("Tests", "Admin");
        }

        private async Task AddQuestion(TestQuestion testQuestion)
        {
            if (ModelState.IsValid)
            {
                await _db.TestQuestions.AddAsync(testQuestion);
                await _db.SaveChangesAsync();
            }
        }
    }
}
