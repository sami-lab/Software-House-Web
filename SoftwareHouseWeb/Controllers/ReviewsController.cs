using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.ViewModel.ReviewViewModel;

namespace SoftwareHouseWeb.Controllers
{
    public class ReviewsController : Controller
    {
        public IReviewRepository reviewRepository;
        public UserManager<ApplicationUser> Usermanager { get; }
        public ReviewsController(IReviewRepository _reviewRepository)
        {
            reviewRepository = _reviewRepository;
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View("PendingReviews",reviewRepository.GetDetails());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult PendingReviews()
        {
            return View(reviewRepository.GetPendingReviews());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ApproveReview(int id)
        {
            bool res = reviewRepository.ApproveReview(id);
             return RedirectToAction("PendingReviews");
         
        }

        public IActionResult Detail(string User_Id)
        {
            var review = reviewRepository.GetDetail(User_Id);
            if (review != null) return View(review);

            ViewBag.ErrorMessage = $"User with Id = {User_Id} cannot be found";
            return View("NotFound");
        }
        public IActionResult AddFeedback()
        {
            var userId= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var review = reviewRepository.GetDetail(userId);
            if (review != null) return View("Detail", review);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFeedback(ReviewViewModel model)
        {
            int id = reviewRepository.addReview(model);
            return RedirectToAction("Profile","Account");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool r = reviewRepository.delete(id);
            if (User.IsInRole("Admin"))
            return RedirectToAction("PendingReviews");
            else
              return RedirectToAction("Profile", "Account");
        }

        public IActionResult Edit(string User_Id)
        {
            var review = reviewRepository.GetDetail(User_Id);
            if (review != null) return View(review);

            ViewBag.ErrorMessage = $"User with Id = {User_Id} cannot be found";
            return View("NotFound");
        }
        public IActionResult Edit(ReviewViewModel model)
        {
            var userid = reviewRepository.Update(model);
            if (userid == "-1") return View("Error");
            return RedirectToAction("Detail", new { User_Id= userid });
        }
    }
}