﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    public class ConstructorController : Controller
    {
        private readonly ApplicationContext _db;

        public ConstructorController(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var algorithms = await _db.Algorithms.ToListAsync();
            return View(algorithms);
        }

        public async Task<IActionResult> Module(string id)
        {
            var algorithm = await _db.Algorithms.SingleOrDefaultAsync(al => al.Tag == id);

            if (algorithm != null)
                return View(algorithm);

            return NotFound();
        }
    }
}