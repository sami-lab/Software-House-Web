using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.ViewModel.Home;

namespace SoftwareHouseWeb.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        utilities util;
        public IHomeRepository HomeRepository;
        public IReviewRepository reviewRepository;
        public HomeController(ApplicationDbContext _context, IHomeRepository homeRepository, IReviewRepository _reviewRepository)
        {
            util = new utilities(_context);
            HomeRepository = homeRepository;
            reviewRepository = _reviewRepository;
        }
       
        public IActionResult Index()
        {
            //var Services = util.GetAllServices();
            //ViewBag.Services = Services;
            //int LogoServiceID = Services.Where(x => x.ServiceName.Contains("Logo Design")).FirstOrDefault().id;
            //var data = HomeRepository.combineDataofSErvice(LogoServiceID);
            //return View(data);
            var data = HomeRepository.IndexData();
            return View(data);
        }

        //[Authorize("Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Logo(int? Ser_Id)
        {
            var Services = util.GetAllServices();
            ViewBag.Services = Services;
            if(Ser_Id == null)
            {
               int LogoServiceID = Services.Where(x => x.ServiceName.Contains("Logo Design")).FirstOrDefault().id;
               var d = HomeRepository.combineDataofSErvice(LogoServiceID);
               return View(d);
            }
            var data = HomeRepository.combineDataofSErvice((int)Ser_Id);
            return View(data);
        }
        public IActionResult WebDesign(int? Ser_Id)
        {
            var Services = util.GetAllServices();
            ViewBag.Services = Services;
            if (Ser_Id == null)
            {
                int LogoServiceID = Services.Where(x => x.ServiceName.Contains("Web Design")).FirstOrDefault().id;
                var d = HomeRepository.combineDataofSErvice(LogoServiceID);
                return View(d);
            }
            var data = HomeRepository.combineDataofSErvice((int)Ser_Id);
            return View(data);
        }
        public IActionResult WebDev(int? Ser_Id)
        {
            var Services = util.GetAllServices();
            ViewBag.Services = Services;
            if (Ser_Id == null)
            {
                int LogoServiceID = Services.Where(x => x.ServiceName.Contains("Web Dev")).FirstOrDefault().id;
                var d = HomeRepository.combineDataofSErvice(LogoServiceID);
                return View(d);
            }
            var data = HomeRepository.combineDataofSErvice((int)Ser_Id);
            return View(data);
        }

        [Authorize]
        public IActionResult CustomQuote()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult CustomQuote(CustomQuoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = HomeRepository.CustomQuote(model);
                if (id > 0) TempData["Message"] = "Custom Quotes saved Successfully";
                return Redirect("Index");
            }
            return View(model);
        }

        public IActionResult HowItWorks(int id)
        {
            string Servicename = util.GetAllServices().FirstOrDefault(x => x.id == id).ServiceName;
            if (Servicename == null) Servicename = "Web Design";
            return View("HowItWorks",Servicename);
        }
        //[Authorize]
        public IActionResult testimonials()
        {
            var reviews = reviewRepository.GetDetails();
            return View(reviews);
        }

        public IActionResult WhyUs()
        {
            return View();
        }
        public IActionResult OurGurantee()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult SiteMap()
        {
            return View();
        }
        public JsonResult GetAllServices()
        {
            var Ser = util.GetAllServices();
            return Json(Ser);
        }
    }
}