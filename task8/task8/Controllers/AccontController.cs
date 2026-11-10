using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using task8.Models;

namespace task8.Controllers
{
    public class AccontController : Controller

    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccontController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO reg)
        {
            if (!ModelState.IsValid)
            {
                return View(reg);
            }

            // إنشاء الدور إذا مش موجود
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole("User"));

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(reg);
                }
            }

            var user = new IdentityUser
            {
                UserName = reg.Email,
                Email = reg.Email
            };

            var result = await _userManager.CreateAsync(user, reg.Password);

            if (result.Succeeded)
            {
                var assignResult = await _userManager.AddToRoleAsync(user, "User");

                if (assignResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in assignResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                // التراجع عن الحساب الجديد إذا فشل إعطاء الدور
                var deleteResult = await _userManager.DeleteAsync(user);

                if (!deleteResult.Succeeded)
                {
                    ModelState.AddModelError(
                        "", "The account was created, but its role could not be assigned.");
                }

                return View(reg);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(reg);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(
                login.Email,
                login.Password,
                false,
                false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(
                "", "Login failed. Check your email and password.");

            return View(login);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

    }
}
