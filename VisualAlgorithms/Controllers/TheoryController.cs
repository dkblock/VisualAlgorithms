using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    public class TheoryController : Controller
    {
        private readonly ApplicationContext _db;

        public TheoryController(ApplicationContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var algorithms = _db.Algorithms.ToList();
            return View(algorithms);
        }

        [HttpGet]
        public IActionResult Info(int id)
        {
            var algorithm = _db.Algorithms.Find(id);
            return View(algorithm);
        }
    }
}
