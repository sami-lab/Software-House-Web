using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.ViewModel;
using SoftwareHouseWeb.ViewModel.Portfolio;

namespace SoftwareHouseWeb.Controllers
{
    public class PortfolioController : Controller
    {
        public IPortfolioRepository portfolioRepository;
        public utilities util;
        public PortfolioController(ApplicationDbContext context, IPortfolioRepository _portfolioRepository)
        {
            portfolioRepository = _portfolioRepository;
            util= new  utilities(context);
        }
        public IActionResult Index()
        {
            var Service = util.GetAllServices();
            ViewBag.Services = Service;
            return View(portfolioRepository.GetDetails());
        }
        public IActionResult ServicePortfolio(int? id)
        {
            if (id != null)
            {
                var Service = util.GetAllServices().FirstOrDefault(x => x.id == id); ;
                if(Service == null) return Redirect("Index");
                ViewBag.Ser_id = Service.id;
                var p = portfolioRepository.ServicePortfolio((int)id);
                return View(p);
            }
            return Redirect("Index");
        }

        [HttpPost]
        public IActionResult Create(PortfolioViewModel model)
        {
            if (ModelState.IsValid)
            {
            int id = portfolioRepository.addPortfolio(model);
            return RedirectToAction("ServicePortfolio",new { Ser_Id = model.Ser_Id });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            portfolioRepository.delete(id);
            return RedirectToAction("Index");
        }
    }
}