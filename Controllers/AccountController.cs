﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueenOfApostlesRenewalCentre.Models;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Set SelectedRole to "User" if it's null or empty
            if (string.IsNullOrEmpty(model.SelectedRole))
            {
                model.SelectedRole = "User";
            }

            // Check for and show validation errors
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Always assign User role for public registration
                    await _userManager.AddToRoleAsync(user, "User");

                    // Sign in the user immediately after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Add success message to TempData for display on redirect
                    TempData["SuccessMessage"] = "Registration successful!";

                    // Redirect to home page
                    return RedirectToAction("Index", "Home");
                }

                // If we got this far, something failed, redisplay form
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception during registration: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred during registration: " + ex.Message);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var isStaff = await _userManager.IsInRoleAsync(user, "Staff");

            if ( isAdmin || isStaff)
            {
                return View("~/Areas/Staff/Views/StaffDashboard/Profile.cshtml", user);
            }

            return View("Profile", user);
            
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login");
            }


            var model = new UserProfileViewModel {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,

            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model) {

            Console.WriteLine(model.Name);
            Console.WriteLine(model.Surname);
            Console.WriteLine(model.Email);


            if (!ModelState.IsValid) {
                return View(model);
            }


            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return RedirectToAction("Login"); }

            user.Name = model.Name;
            user.Surname = model.Surname;

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase)) {
                var emailExists = await _userManager.FindByEmailAsync(model.Email);
                if (emailExists != null && emailExists.Id != user.Id) {
                    ModelState.AddModelError("Email", "This email is already in use by another account.");
                    return View(model);
                }

                user.Email = model.Email;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) {
                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }


        [HttpGet]
        public IActionResult ChangePassword() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) {


            if(!ModelState.IsValid) {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded) {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Your password has been changed successfully.";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }


    }
}