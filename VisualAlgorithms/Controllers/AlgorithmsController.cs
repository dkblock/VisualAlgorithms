using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    public class AlgorithmsController : Controller
    {
        private readonly ApplicationContext _db;

        public AlgorithmsController(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var algorithms = await _db.Algorithms.ToListAsync();
            return View(algorithms);
        }

        [HttpGet]
        public async Task<IActionResult> Info(string id)
        {
            var algorithm = await _db.Algorithms
                .Include(al => al.AlgorithmTimeComplexity)
                .SingleOrDefaultAsync(al => al.Tag == id);

            if (algorithm != null)
                return View(algorithm);

            return NotFound();
        }
    }
}