using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.AppMiddleware;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    public class TestsController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TestsManager _testsManager;

        public TestsController(
            ApplicationContext db, 
            UserManager<ApplicationUser> userManager,
            TestsManager testsManager)
        {
            _db = db;
            _userManager = userManager;
            _testsManager = testsManager;
        }

        public async Task<IActionResult> Index()
        {
            var tests = await _db.Tests
                .Include(t => t.TestQuestions)
                .Include(t => t.Algorithm)
                .ToListAsync();

            return View(tests);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Algorithms = await _db.Algorithms.ToListAsync();
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Create(Test test)
        {
            if (ModelState.IsValid)
            {
                var result = await _db.Tests.AddAsync(test);
                await _db.SaveChangesAsync();

                return RedirectToAction("Create", "TestQuestion", new { testId = result.Entity.Id });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Info(int id)
        {
            var test = await _db.Tests
                .Include(t => t.TestQuestions)
                .Include(t => t.Algorithm)
                .SingleAsync(t => t.Id == id);

            return View(test);
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> Passing(int testId, int? questionId)
        {
            if (questionId == null)
            {
                var userId = _userManager.GetUserId(User);
                await _testsManager.ClearUserTestResult(testId, userId);
            }

            var test = await _db.Tests
                .Include(t => t.TestQuestions)
                .SingleAsync(t => t.Id == testId);

            var testQuestion = questionId == null
                ? test.TestQuestions.First()
                : test.TestQuestions.Single(q => q.Id == questionId);

            var userAnswer = new UserAnswer
            {
                TestQuestion = testQuestion,
                TestQuestionId = testQuestion.Id
            };

            return View(userAnswer);
        }

        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public async Task<IActionResult> OnNextQuestionPassing(UserAnswer userAnswer)
        {
            userAnswer.UserId = GetUserId();
            await _testsManager.AddUserAnswer(userAnswer);

            var question = await _db.TestQuestions
                .Include(q => q.Test)
                .ThenInclude(t => t.TestQuestions)
                .SingleAsync(q => q.Id == userAnswer.TestQuestionId);
            var test = question.Test;
            var nextQuestion = test.TestQuestions
                .OrderBy(tq => tq.Id)
                .SkipWhile(q => q.Id != question.Id)
                .Skip(1)
                .First();

            return RedirectToAction("Passing", new { testId = test.Id, questionId = nextQuestion.Id });
        }

        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public async Task<IActionResult> OnEndTestPassing(UserAnswer userAnswer)
        {
            userAnswer.UserId = GetUserId();
            await _testsManager.AddUserAnswer(userAnswer);
            var result = await _testsManager.GetUserTestResult(userAnswer);

            return RedirectToAction("Result", new { testId = result.TestId, userId = result.UserId });
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> Result(int testId, string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            var userTest = await _db.UserTests.FindAsync(testId, userId);

            if (currentUser.Id != userId && !currentRoles.Contains("admin"))
                return RedirectToAction("Index");

            return View(userTest);
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }
    }
}