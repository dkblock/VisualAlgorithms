using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            var tests = _db.Tests.ToList();
            var algorithms = _db.Algorithms.ToList();

            var testViews = tests.Select(test => new TestViewModel
            {
                Id = test.Id,
                TestName = test.Name,
                AlgorithmName = algorithms.Single(al => al.Id == test.AlgorithmId).Name
            });

            return View(testViews);
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

                await _db.Tests.AddAsync(test);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Test(int id)
        {

            return View();
        }
    }
}
