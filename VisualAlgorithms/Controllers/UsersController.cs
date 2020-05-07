using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int? groupId)
        {
            var groups = await _db.Groups.ToListAsync();
            groups.Insert(0, new Group {Id = 0, Name = "Все"});

            var usersList = new List<UserViewModel>();
            var users = await _db.Users
                .Include(u => u.Group)
                .OrderBy(u => u.Group.Name)
                .ThenBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();

            if (groupId != null && groupId != 0)
                users = users.Where(u => u.GroupId == groupId).ToList();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var userModel = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    GroupId = user.GroupId,
                    Role = userRoles.FirstOrDefault(),
                    Groups = groups
                };

                usersList.Add(userModel);
            }

            var usersModel = new UsersViewModel
            {
                Users = usersList,
                GroupId = groupId,
                Groups = groups
            };

            return View(usersModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _db.Users
                .Include(u => u.Group)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var userModel = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    GroupId = user.GroupId,
                    Role = userRoles.FirstOrDefault(),
                    Groups = await _db.Groups.ToListAsync(),
                    Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync()
                };

                return View(userModel);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.FindAsync(userModel.Id);
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.GroupId = userModel.GroupId;

                if (!string.IsNullOrEmpty(userModel.Role))
                {
                    var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _db.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(user, userModel.Role);
                }

                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user != null)
                return View(user);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userToDelete = await _db.Users.FindAsync(user.Id);
            var currentUserId = _userManager.GetUserId(User);

            if (userToDelete == null || userToDelete.Id == currentUserId)
                return RedirectToAction("Index");

            _db.Users.Remove(userToDelete);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}