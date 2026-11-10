using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;


namespace Task8.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var oldRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, oldRoles);

            await _userManager.AddToRoleAsync(user, newRole);

            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }
    }
}