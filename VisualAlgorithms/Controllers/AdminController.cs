using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        public IActionResult Tests()
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
    }
}
