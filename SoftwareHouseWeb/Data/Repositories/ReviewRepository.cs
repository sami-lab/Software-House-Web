using Microsoft.AspNetCore.Http;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Review;
using SoftwareHouseWeb.ViewModel.ReviewViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        public ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ReviewRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
            context = _context;
        }
        public int addReview(ReviewViewModel c)
        {
            Review model = new Review()
            {
                created_At= DateTime.Now,
                Desc = c.Desc,
                Rating= c.Rating,
                User_Id= httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                ReviewStaus= ReviewStatus.WaitingForApproval
            };
            context.Reviews.Add(model);
            context.SaveChanges();
            return model.id;
        }

        public bool delete(int id)
        {
            var result = context.Reviews.FirstOrDefault(u => u.id == id);
            if (result != null)
            {
                context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public ReviewViewModel GetDetail(string User_Id)
        {
            var result = context.Reviews.Select(x => new ReviewViewModel()
            {
               id = x.id,
               created_At = x.created_At,
               Desc = x.Desc,
               Rating=x.Rating,
               User_Id= x.User_Id,
               UserName= x.User.Name,
               UserPhoto= x.User.Photopath,
               reviewStatus= x.ReviewStaus
            }).FirstOrDefault(x => x.User_Id == User_Id);
            return result;
        }

        public List<ReviewViewModel> GetDetails()
        {
            var result = context.Reviews.Where(x=> x.ReviewStaus ==ReviewStatus.Approve).Select(x => new ReviewViewModel()
            {
                id = x.id,
                created_At = x.created_At,
                Desc = x.Desc,
                Rating = x.Rating,
                User_Id = x.User_Id,
                UserName = x.User.Name,
                UserPhoto= x.User.Photopath,
                reviewStatus= x.ReviewStaus 
            }).ToList();
            return result;
        }
        public  List<ReviewViewModel> GetPendingReviews()
        {
                var result = context.Reviews.Where(x => x.ReviewStaus == ReviewStatus.WaitingForApproval).Select(x => new ReviewViewModel()
                {
                    id = x.id,
                    Desc = x.Desc,
                    created_At = x.created_At,
                    Rating = x.Rating,
                    User_Id = x.User_Id,
                    UserName = x.User.Name,
                    UserPhoto= x.User.Photopath,
                    reviewStatus= x.ReviewStaus
                }).ToList();
                return result;
        }

        public bool ApproveReview(int ReviewId)
        {
            try
            {
                var result = context.Reviews.FirstOrDefault(x => x.id == ReviewId && x.ReviewStaus== ReviewStatus.WaitingForApproval);
                if (result != null)
                {
                    result.ReviewStaus = ReviewStatus.Approve;
                    context.Reviews.Attach(result);
                    context.Entry(result).Property("ReviewStaus").IsModified = true;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public string Update(ReviewViewModel c)
        {
            var data = context.Reviews.Find(c.id);
            if (data != null)
            {
                data.Desc = c.Desc;
                data.Rating = c.Rating;
                data.Desc = c.Desc;
               
                context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return data.User_Id;
            }
            return "-1";
        }
    }
}
