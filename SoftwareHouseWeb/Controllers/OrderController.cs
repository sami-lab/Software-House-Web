using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Orders;
using SoftwareHouseWeb.ViewModel.Administration;
using SoftwareHouseWeb.ViewModel.OrderViewModel;
using SoftwareHouseWeb.ViewModel.PackagesViewModel;

namespace SoftwareHouseWeb.Controllers
{
    public class OrderController : Controller
    {
        public IOrderRepository OrderRepository;
        utilities util;
        public UserManager<ApplicationUser> UserManager;
        public IPackagesRepository packagesRepository;
        public IEmployeeRepository employeeRepository;
        public OrderController(ApplicationDbContext context, IOrderRepository orderRepository, UserManager<ApplicationUser> _UserManager, IPackagesRepository _packagesRepository, IEmployeeRepository _employeeRepository)
        {
            util = new utilities(context);
            UserManager = _UserManager;
            OrderRepository = orderRepository;
            packagesRepository = _packagesRepository;
            employeeRepository = _employeeRepository;
        }
        public IActionResult Index()
        {
            var orders = OrderRepository.Details();
            return View(orders);
        }
       
        [HttpPost]
        public IActionResult ShowSummary(OrderViewModel model)
        {
            try
            {
               var package= JsonConvert.DeserializeObject<List<PackagesViewModel>>(model.HoldPackage);
                //var p = model.Packages.SelectMany(x => x.Packages).Select(y => new OrderPackagesViewModel()
                //{
                //    PkgName = y.PkgName,
                //    pkg_Id = y.id,
                //    Ser_Id = y.Ser_Id,
                //    Quantity = y.Quantity,
                //    price = (y.TotalPrice - (y.DiscountPercent / 100 * y.TotalPrice)) * y.Quantity
                //}).ToList();
                var p = package.Where(x => x.is_selected == true).Select(y => new OrderPackagesViewModel()
                {
                    PkgName = y.PkgName,
                    pkg_Id = y.id,
                    Ser_Id = y.Ser_Id,
                    Quantity = y.Quantity,
                    price = (y.TotalPrice - (y.DiscountPercent / 100 * y.TotalPrice)) * y.Quantity
                }).ToList();
                ViewBag.TotalPrice = package.Where(x => x.is_selected == true).Sum(x => x.TotalPrice);
                model.OrderPackages = p;
                return PartialView("OrderSummary", model);
            }
            catch
            {
                return PartialView("OrderSummary", model);
            }
        }


        public IActionResult Create(string promoCode, int? Ser_id, int? Pkg_id)
        {
            ViewBag.Services = util.GetAllServices();
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = UserManager.Users.Include(x => x.User_Communication).FirstOrDefault(x => x.Id == userId);

            OrderViewModel o = new OrderViewModel()
            {
                cus_name = user.Name,
                Email = user.Email,
                Country = user.Country,
                cus_phone = user.PhoneNumber,
            };
            if (user.User_Communication != null)
            {
                o.FirstPreferedTimeStart = user.User_Communication.FirstPreferedTimeStart;
                o.FirstPreferedTimeEnd = user.User_Communication.FirstPreferedTimeEnd;
                o.FirstPreferences = user.User_Communication.FirstPreferences;
                o.SecondPreferences = user.User_Communication.SecondPreferences;
                o.SecondPreferedTimeStart = user.User_Communication.SecondPreferedTimeStart;
                o.SecondPreferedTimeEnd = user.User_Communication.SecondPreferedTimeEnd;
            }
            if (promoCode != null)
            {
                o.PromoCode = promoCode;
            }


            var Packages = packagesRepository.GetDetails().GroupBy(x => new { x.Ser_Id, x.Ser_Name })
                             .Select(x => new GroupByServices() { Ser_Id = x.Key.Ser_Id, Ser_Name = x.Key.Ser_Name, Packages = x.ToList() }).ToList();
            var packages = Packages.SelectMany(x => x.Packages);
            packages.ToList().ForEach(x => x.Quantity = 1);
            if (Pkg_id > 0)
            {
                foreach (var pack in packages.Where(y => y.id == Pkg_id))
                {
                    pack.is_selected = true;
                }
            }
            o.ServicesPackages = Packages;
            return View(o);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var package = JsonConvert.DeserializeObject<List<PackagesViewModel>>(model.HoldPackage);
                    var p = package.Where(x => x.is_selected == true).Select(y => new OrderPackagesViewModel()
                    {
                        PkgName = y.PkgName,
                        pkg_Id = y.id,
                        Ser_Id = y.Ser_Id,
                        Quantity = y.Quantity,
                        price = (y.DiscountPercent / 100 * y.TotalPrice) * y.Quantity
                    }).ToList();
                    model.OrderPackages = p;
                    //-2 for InitialPromo Used already used
                    //-3 for PromoCode Invalid
                    int order_id = await OrderRepository.Create(model);
                    if (order_id == -2 || order_id == -3)
                    {
                        ModelState.AddModelError("", "The Promo Code You Provided is Expired or Invalid");
                        ViewBag.Services = util.GetAllServices();
                        return View(model);
                    }
                    if (order_id == -3)
                    {
                        ModelState.AddModelError("", "No Packages Selected");
                        ViewBag.Services = util.GetAllServices();
                        return View(model);
                    }
                    TempData["Message"] = "Order Successful";
                    TempData.Keep();
                    return RedirectToAction("ChargeAmount", new { order_id = order_id });
                }
                ViewBag.Services = util.GetAllServices();
                return View(model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public IActionResult Detail(int id)
        {
            var order = OrderRepository.Detail(id);
            if (order != null)
            {
                return View(order);
            }
            ViewBag.ErrorMessage = "The resource you are looking for is not available at the moment";
            return View("NotFound");
        }


        public IActionResult UserOrder()
        {
            var userId = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var order = OrderRepository.UserOrders(userId, false);
            return View(order);
        }
        public IActionResult UserUnpaidOrder()
        {
            var userId = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var order = OrderRepository.UserOrders(userId, true);
            return View(order);
        }

        public IActionResult CompletedOrders()
        {
            var orders = OrderRepository.OrdersWithStatus(OrderStatus.Completed);
            return View(orders);
        }
        public IActionResult PendingOrders()
        {
            var orders = OrderRepository.OrdersWithStatus(OrderStatus.Pending);
            return View("CompletedOrders",orders);
        }
        public IActionResult ProgressOrders()
        {
            var orders = OrderRepository.OrdersWithStatus(OrderStatus.Processing);
            return View("CompletedOrders",orders);
        }
        public IActionResult CancelledOrders()
        {
            var orders = OrderRepository.OrdersWithStatus(OrderStatus.Cancelled);
            return View("CompletedOrders",orders);
        }
        public IActionResult RejectedOrders()
        {
            var orders = OrderRepository.OrdersWithStatus(OrderStatus.Completed);
            return View("CompletedOrders",orders);
        }

        [HttpPost] 
        public IActionResult UpdateStatus(UpdateOrderStatus s)
        {
            bool result = OrderRepository.UpdateStatus(s.order_id, s.orderStatus);
            if (result)
            {
                TempData["Message"] = "Status Updated Successfully";
                return RedirectToAction("CompletedOrders");
            }
            else
            {
                TempData["Message"] = "Unable to Update Order Status";
                return RedirectToAction("ProgressOrders");
            }
           

        }
       
        //[Authorize(Roles = "Admin")]
        public PartialViewResult ManageTeam(int order_id)
        {
            try
            {
                var team = OrderRepository.OrderTeam(order_id);
                var emps = employeeRepository.ListEmployees();
                if (team.OrderTeam.Count == 0)
                {
                    var em = emps.Select(x => new Team()
                    {
                        EmployeeName = x.Name,
                        order_id = order_id,
                        Employee_id = x.Employee_id,
                        Position = x.Position,
                        IsSelected = false
                    }).ToList();
                    SetupTeam setupteam = new SetupTeam()
                    {
                        order_id = order_id,
                        Members = em
                    };
                    return PartialView("UpdateTeam", setupteam);
                }

                var Employees = new List<Team>();
                foreach (var emp in emps)
                {
                    Team r = new Team()
                    {
                        Employee_id = emp.Employee_id,
                        EmployeeName = emp.Name,
                        Position = emp.Position,
                        order_id = team.order_id,
                        IsSelected = team.OrderTeam.Any(x => x.emp_id == emp.Employee_id)
                    };
                    Employees.Add(r);
                }
                SetupTeam s = new SetupTeam()
                {
                    StartDate = (DateTime)team.StartDate,
                    EndDate = (DateTime)team.EndDate,
                    order_id = order_id,
                    Members = Employees
                };
                return PartialView("UpdateTeam", s);
            }
            catch (Exception)
            {

                return PartialView("UpdateTeam", null);
            }
        }
        [HttpPost]
        public IActionResult ManageTeam(SetupTeam model)
        {
            bool team = OrderRepository.UpdateTeam(model);
            if(team) TempData["Message"] = "Team Successfully Updated";
            else TempData["Message"] = "Unable To Update Team";
            return RedirectToAction("ProgressOrders");
        }

        public IActionResult ChargeAmount(int order_id)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = "success";
            }
            var order = OrderRepository.Detail(order_id);
            PaymentGatwayViewModel p = new PaymentGatwayViewModel()
            {
                order_id = order.order_id,
                Email = order.Email,
                cus_name = order.cus_name,
                cus_phone = order.cus_phone,
                OrderPackages = order.OrderPackages,
                PaymentMethod = order.PaymentMethod,
                TotalAmount = order.TotalAmount,
            };
            return View(p);
        }
        [HttpPost]
        public IActionResult ChargeAmount(PaymentGatwayViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int order_id = OrderRepository.Charge(model);
                    if (order_id == -1)
                    {
                        ModelState.AddModelError("", "Error in Payment Gatway");
                        return View(model);
                    }
                    else if (order_id == -2)
                    {
                        ViewBag.ErrorMessage = "The resource you are looking for is not available at the moment";
                        return View("NotFound");
                    }
                    return RedirectToAction("Detail", new { id = order_id });
                }
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something Went Wrong! Please Try Again");
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult CancelOrder(int order_id)
        {
            bool res = OrderRepository.CancelOrder(order_id);
            if (res) TempData["Message"] = "success";
            else TempData["Message"] = "fail";
            return RedirectToAction("UserOrder");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            bool res = OrderRepository.Delete(id);
            if (res)
            {
                TempData["Message"] = "Order Deleted Successfully";
            }
            else
            {
                TempData["Message"] = "Unable To delete Order";
            }
            return RedirectToAction("Index");
        }
        //To be continue
        public IActionResult Edit(int id)
        {
            var order = OrderRepository.Detail(id);
            if (order != null)
            {
                var Packages = packagesRepository.GetDetails().GroupBy(x => new { x.Ser_Id, x.Ser_Name })
                             .Select(x => new GroupByServices() { Ser_Id = x.Key.Ser_Id, Ser_Name = x.Key.Ser_Name, Packages = x.ToList() }).ToList();
                var pack = Packages.SelectMany(x => x.Packages).Where(x => order.OrderPackages.Select(t => t.pkg_Id).Contains(x.id));
                pack.ToList().ForEach(x => x.Quantity = 1);
                foreach (var p in pack)
                {
                    p.is_selected = true;
                    p.Quantity = order.OrderPackages.FirstOrDefault(x => x.pkg_Id == p.id).Quantity;
                }

                order.ServicesPackages = Packages;
                return View(order);
            }
            ViewBag.ErrorMessage = "The resource you are looking for is not available at the moment";
            return View("NotFound");
        }
        [HttpPost]   //To be continue(View Not added)
        public IActionResult Edit(OrderViewModel model)
        {
            var package = JsonConvert.DeserializeObject<List<PackagesViewModel>>(model.HoldPackage);
            var p = package.Where(x => x.is_selected == true).Select(y => new OrderPackagesViewModel()
            {
                PkgName = y.PkgName,
                pkg_Id = y.id,
                Ser_Id = y.Ser_Id,
                Quantity = y.Quantity,
                price =   (y.DiscountPercent / 100 * y.TotalPrice) * y.Quantity
            }).ToList();
            model.OrderPackages = p;
            //-1 not found
            //-2 updating paid order
            //-3 for Quantity negative or All Packages null
            int order = OrderRepository.Update(model);
            if (order == -1)
            {
                ViewBag.ErrorMessage = "The resource you are looking for is not available at the moment";
                return View("NotFound");
            }
            else if (order == -2)
            {
                TempData["Message"] = "Unable to Update Paid Order";
                return Redirect("UserOrder");
            }
            else if (order == -3)
            {
                ModelState.AddModelError("", "Please Select at Least one Package");
                return View(model);
            }
            return RedirectToAction("Detail", new { id = order });
        }
        [AcceptVerbs("Get", "Post")]
        public JsonResult quantityCheck(int Quantity)
        {
            if (Quantity > 0)
            {
                return Json(true);
            }
            else
            {
                return Json("Quantity can't be negative");
            }
        }
    }
}