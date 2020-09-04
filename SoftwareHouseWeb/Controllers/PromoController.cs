using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Services.Email;
using SoftwareHouseWeb.ViewModel;
using SoftwareHouseWeb.ViewModel.Promo;

namespace SoftwareHouseWeb.Controllers
{
    public class PromoController : Controller
    {
        public IPromoRepository promoRepository;
        private readonly IEmailSender _emailSender;
        public UserManager<ApplicationUser> UserManager;
        utilities util;
        public PromoController(ApplicationDbContext context, IPromoRepository _promoRepository, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            promoRepository = _promoRepository;
            _emailSender = emailSender;
            UserManager = userManager;
            util = new utilities(context);
        }

        public IActionResult Index()
        {
            var data = promoRepository.ListPromo();
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Services = util.GetAllServices();
            return View();
        }
        [HttpPost]
        public IActionResult Create(PromoViewModel model) 
        {
            if (ModelState.IsValid)
            {
                int id=  promoRepository.AddPromo(model);
                return Redirect("Index");
            }
            ViewBag.Services = util.GetAllServices();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var promo = promoRepository.GetPromo(id);
            if(promo != null)
            {
                ViewBag.Services = util.GetAllServices();
                return View(promo);
            }
            ViewBag.ErrorMessage = "The Promo with id" + id + "Could not be found";
            return View("NotFound");
        }
        [HttpPost]
        public IActionResult Edit(int id,PromoViewModel model)
        {
            if (!ModelState.IsValid)
            {
               ViewBag.Services = util.GetAllServices();
               return View(model);

            }
            var promo = promoRepository.GetPromo(id);
            if (promo != null)
            {
                int ID = promoRepository.Update(id, model);
                return View("Index");
            }
            ViewBag.ErrorMessage = "The Promo with id" + id + "Could not be found";
            return View("NotFound");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            promoRepository.delete(id);
            return View("Index");
        }

        
        [Authorize] //Just for InitalPromo
        public async Task<JsonResult> GetPromoCode(string name,string email,int id)
        {
            if(email.Trim() == null || id <= 0) return Json(false);
            var promo = promoRepository.GetInitialPromo(id);
            if (promo == null) return Json(false);

             var user = await UserManager.GetUserAsync(User);
            if (user.InitialPromoUsed == true) return Json(false);
            var OrderLink = Url.Action("Order", "Create",
                     new { promoCode =  promo.PromoCode,Ser_id = id}, Request.Scheme);
            PromoEmailViewModel p = new PromoEmailViewModel()
            {
                Name = name,
                Url = OrderLink,
                PromoCode= promo.PromoCode,
                PromoDiscount= promo.PromoDiscount
            };
            string str = await ViewToStringRenderer.RenderViewToStringAsync(HttpContext.RequestServices, $"~/Views/Template/Promo.cshtml", p);

            await _emailSender.SendEmailAsync(email, "Promo Code", str);
            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult CheckPromo(string PromoCode)
        {
            var promo=  promoRepository.GetPromoByCode(PromoCode);
            if (promo)
            {
                return Json(true);
            }
            else
            {
                return Json("Invalid Promo Code");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult GetPromoDiscount(string PromoCode)
        {
            var promo = util.CheckPromo(PromoCode);
            if (promo != null)
            {
                return Json(new {Ser_Id=promo.Item1,Discount= promo.Item2});
            }
            else
            {
                return Json("Invalid Promo Code");
            }
        }
    }
}