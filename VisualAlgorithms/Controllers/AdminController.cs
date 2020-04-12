using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;

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

        public async Task<IActionResult> Stats()
        {
            var userTests = await _db.UserTests
                .Include(ut => ut.User)
                .Include(ut => ut.Test)
                .ThenInclude(t => t.Algorithm)
                .ToListAsync();

            return View(userTests);
        }
    }
}
