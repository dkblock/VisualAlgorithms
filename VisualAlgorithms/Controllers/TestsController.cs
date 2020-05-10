using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.AppHelpers;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    public class TestsController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TestsManager _testsManager;

        public TestsController(
            ApplicationContext db,
            IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager,
            TestsManager testsManager)
        {
            _db = db;
            _env = env;
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

                return RedirectToAction("Create", "TestQuestion", new { testId = result.Entity.Id, isNewTest = true });
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Algorithms = await _db.Algorithms.ToListAsync();
            var test = await _db.Tests
                .Include(t => t.TestQuestions)
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync();

            if (test == null)
                return RedirectToAction("Tests", "Admin");

            return View(test);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Test newTest)
        {
            var test = await _db.Tests.FindAsync(newTest.Id);
            test.Name = newTest.Name;
            test.AlgorithmId = newTest.AlgorithmId;

            _db.Entry(test).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return RedirectToAction("Tests", "Admin");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var test = await _db.Tests.FindAsync(id);

            if (test == null)
                return RedirectToAction("Tests", "Admin");

            return View(test);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(Test test)
        {
            var testQuestions = await _db.TestQuestions
                .Where(tq => tq.TestId == test.Id)
                .ToListAsync();

            foreach (var question in testQuestions)
            {
                if (!string.IsNullOrEmpty(question.Image))
                {
                    var path = Path.Combine(_env.WebRootPath, "images", "test-questions", question.Image);
                    System.IO.File.Delete(path);
                }
            }

            _db.Tests.Remove(test);
            await _db.SaveChangesAsync();

            return RedirectToAction("Tests", "Admin");
        }

        public async Task<IActionResult> Info(int id)
        {
            var test = await _db.Tests
                .Include(t => t.TestQuestions)
                .Include(t => t.Algorithm)
                .Include(t => t.UserTests)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (test != null)
                return View(test);

            return NotFound();
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> Passing(int testId, int? questionId)
        {
            if (questionId == null)
                await _testsManager.CheckUserTest(testId, GetUserId());

            if (await _testsManager.CheckUserAnswer(testId, questionId, GetUserId()))
                return RedirectToAction("Index");

            var test = await _db.Tests
                .Include(t => t.TestQuestions)
                .ThenInclude(tq => tq.TestAnswers)
                .SingleOrDefaultAsync(t => t.Id == testId);

            if (test == null) 
                return NotFound();

            var testQuestion = questionId == null
                ? test.TestQuestions.First()
                : test.TestQuestions.SingleOrDefault(q => q.Id == questionId);

            if (testQuestion == null)
                return NotFound();

            ViewBag.QuestionNumber = test.TestQuestions.IndexOf(testQuestion) + 1;
            _testsManager.MixTestAnswers(testQuestion.TestAnswers);

            var userAnswer = new UserAnswer
            {
                TestQuestion = testQuestion,
                TestQuestionId = testQuestion.Id
            };

            return View(userAnswer);

        }

        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public async Task<IActionResult> Passing(UserAnswer userAnswer)
        {
            var userId = GetUserId();
            var question = await _db.TestQuestions
                .Include(q => q.Test)
                .ThenInclude(t => t.TestQuestions)
                .SingleAsync(q => q.Id == userAnswer.TestQuestionId);

            if (await _db.UserAnswers.AnyAsync(ua => ua.UserId == userId && ua.TestQuestionId == question.Id))
                return RedirectToAction("Index");

            userAnswer.UserId = userId;
            await _testsManager.ProcessUserAnswer(userAnswer);

            if (question.IsLastQuestion)
            {
                var result = await _testsManager.GetUserTestResult(userAnswer);
                return RedirectToAction("Result", new { testId = result.TestId, userId = result.UserId });
            }

            var test = question.Test;
            var nextQuestion = test.TestQuestions
                .OrderBy(tq => tq.Id)
                .SkipWhile(q => q.Id != question.Id)
                .Skip(1)
                .First();

            return RedirectToAction("Passing", new { testId = test.Id, questionId = nextQuestion.Id });
        }

        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> PreviousQuestionPassing(int id)
        {
            var currentQuestion = await _db.TestQuestions.FindAsync(id);

            if (currentQuestion == null)
                return NotFound();

            var questions = await _db.TestQuestions
                .Where(tq => tq.TestId == currentQuestion.TestId)
                .ToListAsync();
            var previousQuestion = questions.TakeWhile(q => q.Id != id).LastOrDefault();
            var userId = GetUserId();

            if (previousQuestion == null)
                return NotFound();

            var userAnswer = await _db.UserAnswers.FindAsync(previousQuestion.Id, userId);
            _db.UserAnswers.Remove(userAnswer);
            await _db.SaveChangesAsync();

            return RedirectToAction("Passing", new {testId = previousQuestion.TestId, questionId = previousQuestion.Id});
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