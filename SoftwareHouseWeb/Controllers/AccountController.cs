using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Services.Email;
using SoftwareHouseWeb.ViewModel.Account;

namespace SoftwareHouseWeb.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> Usermanager { get; }
        //public RoleManager<IdentityRole> Rolemanager { get; }
        public SignInManager<ApplicationUser> Signinmanager { get; }
        private readonly IEmailSender _emailSender;
        public IHostingEnvironment hostingEnvironment;
        utilities util;
        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> _Signinmanager, ApplicationDbContext _context, IHostingEnvironment _hostingEnvironment, IEmailSender emailSender)
        {
            Usermanager = usermanager;
            Signinmanager = _Signinmanager;
            hostingEnvironment = _hostingEnvironment;
            util = new utilities(_context, _hostingEnvironment);
            _emailSender = emailSender;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.FirstName + " " + model.LastName,
                    UserName = model.Username,
                    Email = model.Email,
                    JoiningDate = DateTime.Now,
                    //Alternative_Email = model.Alternative_Email,
                    PhoneNumber = model.PhoneNumber,
                    // Alternative_Phone = model.Alternative_Phone,
                    gender = model.gender,
                    Country = model.Country,
                    Photopath = util.ProcessPhotoproperty(model.Photo, "User"),
                };
                var result = await Usermanager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var roles = await Usermanager.AddToRoleAsync(user, "User");
                    var token = await Usermanager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);
                    string str = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices, $"~/Views/Template/Email_Confirmation.cshtml", confirmationLink);

                    await _emailSender.SendEmailAsync(user.Email, "Email Confirmation", str);
                    ViewBag.PageTitle = "Email Confirmation";
                    ViewBag.Title = "Registration successful";
                    ViewBag.Message = "Before you can Login, please confirm your " +
                            "email, by clicking on the confirmation link we have emailed you";
                    return View("EmailConfirmation");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return View(model);
        }


        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailexist(string email)
        {
            var user = await Usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in Use");
            }
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult IsUsernameExist(string Username)
        {
            var user = Usermanager.Users.FirstOrDefault(x => x.UserName == Username);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Username} is already in Use");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "Home");
            }

            var user = await Usermanager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await Usermanager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                string str = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices, $"~/Views/Template/Welcome.cshtml", user.Name);
                await _emailSender.SendEmailAsync(user.Email, "Welcome To Design Guro", str);

                return View("EmailConfirmed");
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                //ExternalLogins = (await Signinmanager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
             //model.ExternalLogins =
            //(await Signinmanager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = await Usermanager.FindByEmailAsync(model.Email);
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                    return View(model);
                }

                if (user != null && !user.EmailConfirmed &&
                            (await Usermanager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }
                var result = await Signinmanager.PasswordSignInAsync(
                    user, model.Password, model.RememberMe,false);


                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        //LocalRedirect(returnUrl);
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (User.IsInRole("Admin"))
                            return RedirectToAction("Admin", "Home");
                        else
                            return RedirectToAction("index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await Usermanager.FindByEmailAsync(model.Email);
                if (user != null && await Usermanager.IsEmailConfirmedAsync(user))
                {
                    var token = await Usermanager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    string str = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices, $"~/Views/Template/ResetPassword.cshtml", passwordResetLink);
                    await _emailSender.SendEmailAsync(user.Email, "Reset Account Password", str);

                    ViewBag.PageTitle = "Email Confirmation";
                    ViewBag.Title = "Password Reset Success";
                    ViewBag.Message = "Before you can Login, please Reset your " +
                            "Password, by clicking on the Reset Password link we have emailed you";
                    return View("EmailConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                ViewBag.PageTitle = "Email Confirmation";
                ViewBag.Title = "Password Reset Success";
                ViewBag.Message = "Before you can Login, please Reset your " +
                        "Password, by clicking on the Reset Password link we have emailed you";
                return View("EmailConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await Usermanager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await Usermanager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await Signinmanager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public  IActionResult ChangePassword(string id)
        {
            //var user = await Usermanager.GetUserAsync(User);
            //var userHasPassword = await Usermanager.HasPasswordAsync(user);
            //if (!userHasPassword)
            //{
            //    return RedirectToAction("AddPassword");
            //}
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await Usermanager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await Usermanager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await Signinmanager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await Usermanager.GetUserAsync(User);
            EditUserViewModel r = new EditUserViewModel()
            {
                id = user.Id,
                Country = user.Country,
                gender = user.gender,
                PhoneNumber = user.PhoneNumber,
                Photopath = user.Photopath,
                FullName = user.Name
            };
            return View(r);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await Usermanager.FindByIdAsync(model.id);
                if (user != null)
                {
                    user.Name = model.FullName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.gender = model.gender;
                    user.Country = model.Country;
                    string uniqueFileName = "";
                    if (model.Photo != null)
                    {
                        if (user.Photopath != null)
                        {
                            string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Image", "User", user.Photopath);
                            System.IO.File.Delete(filepath);
                        }
                        uniqueFileName = util.ProcessPhotoproperty(model.Photo, "User");
                    }
                    var result = await Usermanager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
                ViewBag.ErrorMessage = $"User with Id = {model.id} cannot be found";
                return View("NotFound");

            }
            return View(model);
        }
        [Authorize]
        public  IActionResult Profile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = Usermanager.Users.Include(x => x.User_Communication)
                  .Include(x => x.Review_Model).Include(x => x.Customer).Include(x => x.Customer.Order)
                  .FirstOrDefault(x => x.Id == userId);


            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [Authorize]
        public async Task<IActionResult> UpdateCommunication()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await Usermanager.Users.Include(x=> x.User_Communication).FirstOrDefaultAsync(x=> x.Id== userId);
            if (user.User_Communication != null)
            {
                UpdateCommunicationViewModel u = new UpdateCommunicationViewModel()
                {
                    id = user.User_Communication.id,
                    FirstPreferences = user.User_Communication.FirstPreferences,
                    SecondPreferences = user.User_Communication.SecondPreferences,
                    FirstPreferedTimeStart = user.User_Communication.FirstPreferedTimeStart,
                    FirstPreferedTimeEnd = user.User_Communication.FirstPreferedTimeEnd,
                    SecondPreferedTimeEnd = user.User_Communication.SecondPreferedTimeEnd,
                    SecondPreferedTimeStart = user.User_Communication.SecondPreferedTimeStart
                };
                return View(u);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCommunication(UpdateCommunicationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    var user = await Usermanager.Users.Include(x => x.User_Communication).FirstOrDefaultAsync(x => x.Id == userId);
                    if (user.User_Communication != null)
                    {
                        user.User_Communication.FirstPreferences = model.FirstPreferences;
                        user.User_Communication.SecondPreferences = model.SecondPreferences;
                        user.User_Communication.FirstPreferedTimeStart = model.FirstPreferedTimeStart;
                        user.User_Communication.FirstPreferedTimeEnd = model.FirstPreferedTimeEnd;
                        user.User_Communication.SecondPreferedTimeEnd = model.SecondPreferedTimeEnd;
                        user.User_Communication.SecondPreferedTimeStart = model.SecondPreferedTimeStart;

                    }
                    else
                    {
                        UsersCommunication u = new UsersCommunication()
                        {
                            FirstPreferences = model.FirstPreferences,
                            SecondPreferences = model.SecondPreferences,
                            FirstPreferedTimeStart = model.FirstPreferedTimeStart,
                            FirstPreferedTimeEnd = model.FirstPreferedTimeEnd,
                            SecondPreferedTimeEnd = model.SecondPreferedTimeEnd,
                            SecondPreferedTimeStart = model.SecondPreferedTimeStart,
                        };
                        user.User_Communication = u;
                    }

                    var result = await Usermanager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Profile");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }

                return View(model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

    }
}                 