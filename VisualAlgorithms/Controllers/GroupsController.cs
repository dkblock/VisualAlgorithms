using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;

namespace VisualAlgorithms.Controllers
{
    [Authorize(Roles = "admin")]
    public class GroupsController : Controller
    {
        private readonly ApplicationContext _db;

        public GroupsController(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _db.Groups.OrderBy(g => g.Name).ToListAsync();
            groups.RemoveAll(g => g.Id == 1);

            return View(groups);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var group = new Group();
            return View(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group group)
        {
            if (ModelState.IsValid)
            {
                await _db.Groups.AddAsync(group);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(group);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var group = await _db.Groups.FindAsync(id);

            if (group != null)
                return View(group);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Group newGroup)
        {
            if (ModelState.IsValid)
            {
                var group = await _db.Groups.FindAsync(newGroup.Id);
                group.Name = newGroup.Name;
                group.IsAvailableForRegister = newGroup.IsAvailableForRegister;
                _db.Entry(group).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(newGroup);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _db.Groups.FindAsync(id);

            if (group != null)
                return View(group);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Group group)
        {
            if (ModelState.IsValid)
            {
                var users = await _db.Users.Where(u => u.GroupId == group.Id).ToListAsync();

                foreach (var user in users)
                {
                    var userToEdit = await _db.Users.FindAsync(user.Id);
                    userToEdit.GroupId = 1;
                    _db.Entry(userToEdit).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }

                _db.Groups.Remove(group);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(group);
        }
    }
}