using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    public class TestingController : Controller
    {
        private readonly ApplicationContext _db;

        public TestingController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var tests = _db.Tests.ToList();
            var algorithms = _db.Algorithms.ToList();

            var testViews = tests.Select(test => new TestViewModel
            {
                TestId = test.Id,
                TestName = test.Name,
                AlgorithmName = algorithms.Where(al => al.Id == test.AlgorithmId).Single().Name
            });

            return View(testViews);
        }
    }
}
