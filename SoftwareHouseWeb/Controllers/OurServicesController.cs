using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.ViewModel.ServicesViewModel;

namespace SoftwareHouseWeb.Controllers
{
    public class OurServicesController : Controller
    {
        public IServicesRepository ServicesRepository;
        public OurServicesController(IServicesRepository _ServicesRepository)
        {
            ServicesRepository = _ServicesRepository;
        }
        public IActionResult Index()
        {
           var ser= ServicesRepository.GetDetails();
            return View(ser);
        }

        public IActionResult Details(int id)
        {
            var ser = ServicesRepository.GetDetail(id);
            if (ser == null) return View(ser);


            ViewBag.ErrorMessage = $"Service with Id cannot be found";
            return View("NotFound");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ServicesViewModel s)
        {
            if (ModelState.IsValid)
            {
              var ser = ServicesRepository.addService(s);
                return Redirect("Index");
            }
            return View(s);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool r=  ServicesRepository.delete(id);
            return RedirectToAction("Index");
           
        }

        public IActionResult Edit(int id)
        {
            var review = ServicesRepository.GetDetail(id);
            if (review != null) return View(review);

            ViewBag.ErrorMessage = $"Service with Id cannot be found";
            return View("NotFound");
        }
        public IActionResult Edit(int id,ServicesViewModel model)
        {
            var i = ServicesRepository.Update(id,model);
            if (i == -1)
            {
                ViewBag.ErrorMessage = $"Service with Id cannot be found";
                return View("NotFound");
            }
            return RedirectToAction("Detail", new { id = i });
        }
    }
}