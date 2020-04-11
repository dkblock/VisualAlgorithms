using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    public class TestsController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TestsController(ApplicationContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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

                return RedirectToAction("Create", "TestQuestion", new {testId = result.Entity.Id});
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

        [HttpGet]
        public async Task<IActionResult> Passing(int testId, int? questionId)
        {
            var test = await _db.Tests
                .Include(t => t.TestQuestions)
                .SingleAsync(t => t.Id == testId);
            var testQuestion = questionId == null
                ? test.TestQuestions.First()
                : test.TestQuestions.Single(q => q.Id == questionId);

            var answer = new UserAnswer
            {
                TestQuestion = testQuestion,
                TestQuestionId = testQuestion.Id
            };

            return View(answer);
        }

        [HttpPost]
        public async Task<IActionResult> OnNextQuestionPassing(UserAnswer testAnswer)
        {
            await AddUserAnswer(testAnswer);

            var question = await _db.TestQuestions
                .Include(q => q.Test)
                .ThenInclude(t => t.TestQuestions)
                .SingleAsync(q => q.Id == testAnswer.TestQuestionId);
            var test = question.Test;
            var nextQuestion = test.TestQuestions
                .OrderBy(tq => tq.Id)
                .SkipWhile(q => q.Id != question.Id)
                .Skip(1)
                .First();

            return RedirectToAction("Passing", new {testId = test.Id, questionId = nextQuestion.Id});
        }

        [HttpPost]
        public async Task<IActionResult> OnEndTestPassing(UserAnswer testAnswer)
        {
            await GetTestResult(testAnswer);
            return RedirectToAction("Index");
        }

        private async Task AddUserAnswer(UserAnswer testAnswer)
        {
            testAnswer.UserId = _userManager.GetUserId(User);
            await _db.UserAnswers.AddAsync(testAnswer);
            await _db.SaveChangesAsync();
        }

        private async Task GetTestResult(UserAnswer testAnswer)
        {
            await AddUserAnswer(testAnswer);

            var userId = _userManager.GetUserId(User);
            var question = await _db.TestQuestions
                .Include(q => q.Test)
                .ThenInclude(t => t.TestQuestions)
                .SingleAsync(q => q.Id == testAnswer.TestQuestionId);
            var testQuestions = question.Test.TestQuestions.ToList();
            var userAnswers = await _db.UserAnswers
                .Where(ua => ua.UserId == userId && testQuestions.Select(tq => tq.Id).Contains(ua.TestQuestionId))
                .ToListAsync();
            var count = 0;

            foreach (var userAnswer in userAnswers)
            {
                var answer = userAnswer.Answer;
                var questionAnswer = testQuestions.Single(q => q.Id == userAnswer.TestQuestionId).Answer;

                if (answer == questionAnswer)
                    count++;
            }

            var result = (double)count / testQuestions.Count * 100;
            var userTest = new UserTest
            {
                PassionTime = DateTime.Now,
                Result = result,
                TestId = question.TestId,
                UserId = userId
            };

            await _db.UserTests.AddAsync(userTest);
            await _db.SaveChangesAsync();
        }
    }
}