using SoftwareHouseWeb.ViewModel.ReviewViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IReviewRepository
    {
        List<ReviewViewModel> GetDetails();
        List<ReviewViewModel> GetPendingReviews();
        ReviewViewModel GetDetail(string User_Id);
        int addReview(ReviewViewModel c);
        bool ApproveReview(int ReviewId);
        string Update(ReviewViewModel emp);
        bool delete(int id);
    }
}
