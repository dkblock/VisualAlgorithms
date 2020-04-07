using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    public class TestsController : Controller
    {
        private readonly ApplicationContext _db;

        public TestsController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var tests = _db.Tests
                .Include(t => t.Questions)
                .Include(t => t.Algorithm)
                .ToList();

            return View(tests);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateTestViewModel
            {
                Algorithms = _db.Algorithms.ToList()
            };
            
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task <ActionResult> Create([Bind("TestName", "AlgorithmId")] CreateTestViewModel testModel)
        {
            if (ModelState.IsValid)
            {
                var test = new Test
                {
                    Name = testModel.TestName,
                    AlgorithmId = testModel.AlgorithmId
                };

                var result = await _db.Tests.AddAsync(test);
                await _db.SaveChangesAsync();

                return RedirectToAction("Create", "TestQuestion", new { testId = result.Entity.Id });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Test(int id)
        {
            var test = await _db.Tests.FindAsync(id);
            var algorithm = await _db.Algorithms.SingleAsync(al => al.Id == test.AlgorithmId);
            var questionsCount = _db.TestQuestions.Count(q => q.TestId == id);

            var testModel = new TestViewModel
            {
                Id = test.Id,
                TestName = test.Name,
                AlgorithmName = algorithm.Name,
                QuestionsCount = questionsCount
            };

            return View(testModel);
        }
    }
}
