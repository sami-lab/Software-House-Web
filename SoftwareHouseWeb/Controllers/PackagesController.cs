using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.ViewModel.PackagesViewModel;

namespace SoftwareHouseWeb.Controllers
{
    public class PackagesController : Controller
    {
        public readonly IPackagesRepository packagesRepository;
        utilities util;
        public PackagesController(IPackagesRepository _packagesRepository,ApplicationDbContext _context)
        {
            packagesRepository = _packagesRepository;
            util = new utilities(_context);
        }
        public IActionResult Index()
        {
            ViewBag.Services = util.GetAllServices();
           var Packages= packagesRepository.GetDetails().GroupBy(x => new { x.Ser_Id, x.Ser_Name })
                                .Select(x => new GroupByServices() { Ser_Id = x.Key.Ser_Id,Ser_Name=x.Key.Ser_Name, Packages = x.ToList() });
            return View(Packages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = util.GetAllServices();
            return View();
        }

        [HttpPost]
        public IActionResult Create(PackagesViewModel model)
        {
            if (model.Desc == null || model.Desc.Split("`", StringSplitOptions.None).Length % 3 != 0)
            {
                ViewBag.Services = util.GetAllServices();
                ModelState.AddModelError("", "Error in Adding Packages Features");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    string id = packagesRepository.addPackage(model);
                    return RedirectToAction("Index");
                }
                ViewBag.Services = util.GetAllServices();
                return View(model);
            }
            ViewBag.Services = util.GetAllServices();
            return View(model);

        }

        public IActionResult ServicePackages(int Ser_Id)
        {
            var packages=   packagesRepository.GetPackagesOfService(Ser_Id);
            return View(packages);
        }

      
        public IActionResult Detail(string id)
        {
            var package = packagesRepository.GetDetail(id);
            if(package != null)
            {
              //ViewBag.Services = util.GetAllServices();
               return View(package);
            }
            ViewBag.ErrorMessage = "The resource you are looking for is not available at the moment";
            return View("NotFound");
        }
        public IActionResult Edit(string id)
        {
            var package = packagesRepository.GetDetail(id);
            if (package != null)
            {
                ViewBag.Services = util.GetAllServices();
                return View(package);
            }
            ViewBag.ErrorMessage = "The resource you are looking for is not available at the moment";
            return View("NotFound");
        }
        [HttpPost]
        public IActionResult Edit(string id,PackagesViewModel model)
        {
            if(model.Desc == null || model.Desc.Split("`",StringSplitOptions.None).Length % 3 != 0)
            {
                ViewBag.Services = util.GetAllServices();
                ModelState.AddModelError("", "Error in Adding Packages Features");
                return View(model);
            }
            string ID = packagesRepository.Update(id, model);
            if(id != "-1")
            {
                return RedirectToAction("Index");
            }
            ViewBag.Services = util.GetAllServices();
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var res = packagesRepository.delete(id);
            return RedirectToAction("Index");
        }
    }
}