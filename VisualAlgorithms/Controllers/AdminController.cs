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
            algorithms.Insert(0, new Algorithm { Id = 0, Name = "Все" });

            var tests = await _db.Tests.ToListAsync();
            tests.Insert(0, new Test { Id = 0, Name = "Все" });

            var groups = await _db.Groups.OrderBy(g => g.Name).ToListAsync();
            groups.Insert(0, new Group { Id = 0, Name = "Все" });

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
    }
}