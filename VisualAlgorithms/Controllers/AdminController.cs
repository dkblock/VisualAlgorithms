using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationContext _db;

        public AdminController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Tests()
        {
            var tests = await _db.Tests
                .Include(t => t.Algorithm)
                .Include(t => t.TestQuestions)
                .ToListAsync();

            return View(tests);
        }

        [HttpGet]
        public async Task<IActionResult> Stats(int? algorithmId, int? testId, int? groupId, int? orderBy)
        {
            var algorithms = await _db.Algorithms.ToListAsync();
            algorithms.Insert(0, new Algorithm {Id = 0, Name = "Все"});

            var tests = await _db.Tests.ToListAsync();
            tests.Insert(0, new Test {Id = 0, Name = "Все"});

            var groups = await _db.Groups.OrderBy(g => g.Name).ToListAsync();
            groups.Insert(0, new Group {Id = 0, Name = "Все"});

            var userTests = await _db.UserTests
                .Include(ut => ut.User)
                .Include(ut => ut.Test)
                .ThenInclude(t => t.Algorithm)
                .OrderByDescending(ut => ut.PassingTime)
                .ToListAsync();

            if (algorithmId != null && algorithmId != 0)
                userTests = userTests.Where(ut => ut.Test.AlgorithmId == algorithmId).ToList();

            if (testId != null && testId != 0)
                userTests = userTests.Where(ut => ut.Test.Id == testId).ToList();

            if (groupId != null && groupId != 0)
                userTests = userTests.Where(ut => ut.User.GroupId == groupId).ToList();

            if (orderBy != null && orderBy != 0)
                userTests = userTests.OrderBy(ut => ut.PassingTime).ToList();
            else
                userTests = userTests.OrderByDescending(ut => ut.PassingTime).ToList();

            var statsModel = new AdminStatsViewModel
            {
                UserTests = userTests,
                Algorithms = algorithms,
                Tests = tests,
                Groups = groups,
                AlgorithmId = algorithmId,
                TestId = testId,
                GroupId = groupId,
                OrderBy = orderBy
            };

            return View(statsModel);
        }

        [HttpPost]
        public async Task<IActionResult> Stats(AdminStatsViewModel statsModel)
        {
            var userTest = await _db.UserTests.FindAsync(statsModel.DeletedTestId, statsModel.DeletedUserId);

            if (userTest != null)
            {
                var userAnswers = await _db.UserAnswers
                    .Include(ua => ua.TestQuestion)
                    .Where(ua =>
                        ua.UserId == statsModel.DeletedUserId && ua.TestQuestion.TestId == statsModel.DeletedTestId)
                    .ToListAsync();

                _db.UserAnswers.RemoveRange(userAnswers);
                _db.UserTests.Remove(userTest);
                await _db.SaveChangesAsync();

                return RedirectToAction("Stats", new
                {
                    algorithmId = statsModel.AlgorithmId,
                    testId = statsModel.TestId,
                    groupId = statsModel.GroupId,
                    orderBy = statsModel.OrderBy
                });
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> TestStats(int id)
        {
            var test = await _db.Tests
                .Include(t => t.TestQuestions)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (test == null)
                return NotFound();

            var userTests = await _db.UserTests
                .Where(ut => ut.TestId == id)
                .ToListAsync();

            var averageTestResult = userTests.Count > 0 ? userTests.Select(ut => ut.Result).Average() : 0;
            var testStatsModel = new TestStatsViewModel
            {
                Test = test,
                TestQuestions = new List<TestQuestionStatsViewModel>(),
                AverageResult = (int) averageTestResult,
                TotalPassings = userTests.Count
            };

            foreach (var question in test.TestQuestions)
            {
                var userAnswers = await _db.UserAnswers
                    .Where(ua => ua.TestQuestionId == question.Id)
                    .ToListAsync();

                var correctAnswers = userAnswers.Count(ua => ua.IsCorrect);
                var incorrectAnswers = userAnswers.Count(ua => !ua.IsCorrect);
                var averageQuestionResult = userAnswers.Count > 0 ? (double) correctAnswers / userAnswers.Count * 100 : 0;
                var questionStatsModel = new TestQuestionStatsViewModel
                {
                    TestQuestion = question,
                    AverageResult = (int) averageQuestionResult,
                    CorrectAnswers = correctAnswers,
                    IncorrectAnswers = incorrectAnswers,
                    TotalAnswers = userAnswers.Count
                };

                testStatsModel.TestQuestions.Add(questionStatsModel);
            }

            return View(testStatsModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserTestStats(int testId, string userId, int? answersType)
        {
            var answerTypes = new List<string> {"Все ответы", "Только верные ответы", "Только неверные ответы"};
            var userTest = await _db.UserTests
                .Include(ut => ut.Test)
                .ThenInclude(t => t.TestQuestions)
                .SingleOrDefaultAsync(ut => ut.TestId == testId && ut.UserId == userId);

            if (userTest == null)
                return NotFound();

            var user = await _db.Users.Include(u => u.Group)
                .SingleOrDefaultAsync(u => u.Id == userId);

            var testQuestions = await _db.TestQuestions
                .Where(tq => tq.TestId == testId)
                .Select(tq => tq.Id)
                .ToListAsync();

            var allUserAnswers = await _db.UserAnswers
                .Include(ua => ua.TestQuestion)
                .ThenInclude(tq => tq.TestAnswers)
                .Where(ua => ua.UserId == userId && testQuestions.Contains(ua.TestQuestionId))
                .ToListAsync();

            var userTestStatsModel = new UserTestStatsViewModel
            {
                User = user,
                UserTest = userTest,
                AllUserAnswers = allUserAnswers,
                AnswersType = answersType,
                AnswerTypes = answerTypes
            };

            if (answersType != null && answersType != 0)
                userTestStatsModel.UserAnswers = answersType == 1
                    ? allUserAnswers.Where(ua => ua.IsCorrect).ToList()
                    : allUserAnswers.Where(ua => !ua.IsCorrect).ToList();
            else
                userTestStatsModel.UserAnswers = allUserAnswers;

            return View(userTestStatsModel);
        }
    }
}