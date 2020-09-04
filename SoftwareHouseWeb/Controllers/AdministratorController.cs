using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Services.Email;
using SoftwareHouseWeb.ViewModel.Administration;

namespace SoftwareHouseWeb.Controllers
{
    [AllowAnonymous]
    public class AdministratorController : Controller
    {
        public UserManager<ApplicationUser> Usermanager { get; }
        public RoleManager<IdentityRole> Rolemanager { get; }
        public SignInManager<ApplicationUser> Signinmanager { get; }
        private readonly IEmailSender _emailSender;
        public IEmployeeRepository employeeRepository;
        public IHomeRepository homeRepository;
        utilities util;
        public AdministratorController(UserManager<ApplicationUser> usermanager,RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signinmanager, ApplicationDbContext db, IHostingEnvironment hostingEnvironment, IEmailSender emailSender, IEmployeeRepository _employeeRepository,IHomeRepository _homeRepository)
        {
            Usermanager = usermanager;
            Rolemanager = roleManager;
            Signinmanager = signinmanager;
            _emailSender = emailSender;
            employeeRepository = _employeeRepository;
            homeRepository = _homeRepository;
            util = new utilities(db, hostingEnvironment);
        }
        //[Authorize(Roles = "Super Admin,Admin")]
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        //[Authorize(Roles = "Super Admin,Admin")]
        public IActionResult AddEmployee(RegisterEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = employeeRepository.AddEmployee(model);
                return Redirect("ListEmployees");
            }
            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(CreateRoleViewmodel model)
        {
            var identityRole = new IdentityRole
            {
                Name = model.RoleName
            };
            var result = await Rolemanager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("GetAllRoles", "Administrator");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult GetAllRoles()
        {
            var model = Rolemanager.Roles;
            return View(model);
        }

       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string id)
        {
            // Find the role by Role ID
            var role = await Rolemanager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new CreateRoleViewmodel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(CreateRoleViewmodel model)
        {
            var role = await Rolemanager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await Rolemanager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await Rolemanager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await Rolemanager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAllRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("GetAllRoles");
                }
                catch
                {
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return View("Error");
                }
            }
        }
        //[Authorize(Roles = "Admin,Super Admin,Employee")]
        public IActionResult ListUsers()
        {
            var model = Usermanager.Users;
            return View(model);
        }
        public IActionResult ListUsersWithOrders()
        {
            var model = Usermanager.Users.Where(x =>  x.Customer.Order != null);
            return View(model);
        }

        public IActionResult ListEmployees()
        {
            var model = employeeRepository.ListEmployees();
            return View(model);
        }
        public IActionResult ListInterns()
        {
            var model = employeeRepository.ListInterns();
            return View("ListEmployees",model);
        }


        public IActionResult DeleteEmployee(int id)
        {
            bool emp=  employeeRepository.delete(id);
            return RedirectToAction("ListEmployees");
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = await Usermanager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await Usermanager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Redirect("~/Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Redirect("~/Home");
            }
        }

        public IActionResult CustomQuotes()
        {
            var data = homeRepository.GetAllCustomQuotes();
            return View(data);
        }
    }
}