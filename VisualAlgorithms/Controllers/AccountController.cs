using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ApplicationContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var groups = await _db.Groups
                .Where(g => g.IsAvailableForRegister)
                .OrderBy(g => g.Name)
                .ToListAsync();
            var registerModel = new RegisterViewModel {Groups = groups};

            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    GroupId = model.GroupId
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Stats()
        {
            var userId = _userManager.GetUserId(User);
            var userTests = await _db.UserTests
                .Include(ut => ut.Test)
                .ThenInclude(t => t.Algorithm)
                .Where(ut => ut.User.Id == userId)
                .OrderByDescending(ut => ut.PassingTime)
                .ToListAsync();

            return View(userTests);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Settings()
        {
            var userId = _userManager.GetUserId(User);
            var passwordModel = new ChangePasswordViewModel {Id = userId};

            return View(passwordModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Settings(ChangePasswordViewModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(passwordModel.Id);

                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, passwordModel.OldPassword,
                        passwordModel.NewPassword);

                    if (result.Succeeded)
                        return RedirectToAction("Index");

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }

            return View(passwordModel);
        }
    }
}