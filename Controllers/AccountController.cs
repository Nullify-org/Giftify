using Giftify.Models;
using Giftify.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers
{
    public class AccountController : Controller
    {
       
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(
            UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
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
        public async Task<IActionResult> Register(RegisterVM userFromReq)
        {
            if (!ModelState.IsValid)
                return View(userFromReq);

            ApplicationUser user = new ApplicationUser()
            {
                FullName = userFromReq.FullName,
                UserName = userFromReq.UserName,
                Email = userFromReq.Email,
                DateOfBirth = userFromReq.DateOfBirth,
                City = userFromReq.City,
                Street = userFromReq.Street,
                Building = userFromReq.Building,
                AddressNotes = userFromReq.AddressNotes
            };

            IdentityResult result = await userManager.CreateAsync(user, userFromReq.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(" Register", userFromReq);
        }











    }
}
