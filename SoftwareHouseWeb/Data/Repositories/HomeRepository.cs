using Microsoft.AspNetCore.DataProtection;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.CustomQuote;
using SoftwareHouseWeb.Security.Tokens;
using SoftwareHouseWeb.ViewModel.Home;
using SoftwareHouseWeb.ViewModel.ReviewViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        public ApplicationDbContext context;
        private readonly IDataProtector protector;
        public HomeRepository(ApplicationDbContext _context, IDataProtectionProvider dataProtectionProvider, DataProctectionPurposeString dataProtectionPurposeStrings)
        {
            context = _context;
            this.protector = dataProtectionProvider.CreateProtector(
           dataProtectionPurposeStrings.PurposeString);
        }
        public int CustomQuote(CustomQuoteViewModel model)
        {
            CustomQuote c = new CustomQuote()
            {
                Name = model.Name,
                Country = model.Country,
                Date = DateTime.Now,
                Email = model.Email,
                Message = model.Message,
                PhoneNumber = model.PhoneNumber
            };
            context.CustomQuotes.Add(c);
            context.SaveChanges();
            return c.id;
        }

        public IndexViewModel IndexData()
        {
            var IndexView = context.Services.Select(x => new IndexView()
            {
                Services = new ViewModel.ServicesViewModel.ServicesViewModel()
                {
                    id = x.id,
                    ServiceName = x.ServiceName,
                    IconPath = x.IconPath
                },
                Packages = x.Packages.Where(t => t.isActive == true).Select(t => new ViewModel.PackagesViewModel.PackagesViewModel()
                {
                    id = t.id,
                    Encrypted_id = protector.Protect(x.id.ToString()),
                    Desc = t.Desc,
                    TotalPrice = t.TotalPrice,
                    Ser_Id = t.Ser_Id,
                    Ser_Name = t.Service_Model.ServiceName,
                    DiscountPercent = t.DiscountPercent,
                    Heading = t.Heading,
                    LaunchDate = t.LaunchDate,
                    PhotoPath = t.PhotoPath,
                    PkgName = t.PkgName,
                    Description = t.Desc.Split("`", StringSplitOptions.None)
                }).OrderByDescending(t => t.LaunchDate.Year)
                .OrderByDescending(t => t.LaunchDate.Month)
                .OrderByDescending(t => t.LaunchDate.Day).Take(1).FirstOrDefault(),
                Portfolios = x.Portfolios.Select(t => new ViewModel.Portfolio.PortfolioViewModel()
                {
                    id = t.id,
                    Desc = t.Desc,
                    Heading = t.Heading,
                    PhotoPath = t.PhotoPath,
                    created_At = t.created_At,
                    Ser_Id = t.Ser_Id,
                }).OrderByDescending(t => t.created_At.Year)
                .OrderByDescending(t => t.created_At.Month)
                .OrderByDescending(t => t.created_At.Day).Take(1).FirstOrDefault(),
            }).ToList();

           var reviews = context.Reviews.Where(x => x.ReviewStaus == Models.Review.ReviewStatus.Approve).Select(x => new ReviewViewModel()
            {
                id = x.id,
                Rating = x.Rating,
                Desc = x.Desc,
                created_At = x.created_At,
                UserName = x.User.UserName,
                UserPhoto = x.User.Photopath,
            }).OrderByDescending(t => t.created_At)
               .Take(5).ToList();
            IndexViewModel v = new IndexViewModel();
            v.IndexView = IndexView;
            v.Reviews = reviews;
            return  v;
        }
        public CombineData combineDataofSErvice(int Ser_Id)
        {
            var data = context.Services.Where(x => x.id == Ser_Id).Select(x => new CombineData()
            {
                Services = new ViewModel.ServicesViewModel.ServicesViewModel()
                {
                    id = x.id,
                    ServiceName = x.ServiceName,
                    IconPath = x.IconPath
                },
                Packages = x.Packages.Where(t=> t.isActive ==true).Select(t => new ViewModel.PackagesViewModel.PackagesViewModel()
                {
                    id = t.id,
                    Encrypted_id = protector.Protect(x.id.ToString()),
                    Desc = t.Desc,
                    TotalPrice = t.TotalPrice,
                    Ser_Id = t.Ser_Id,
                    Ser_Name= t.Service_Model.ServiceName,
                    DiscountPercent = t.DiscountPercent,
                    Heading = t.Heading,
                    LaunchDate = t.LaunchDate,
                    PhotoPath = t.PhotoPath,
                    PkgName = t.PkgName,
                    Description = t.Desc.Split("`", StringSplitOptions.None)
                }).OrderByDescending(t => t.LaunchDate.Year)
                .OrderByDescending(t => t.LaunchDate.Month)
                .OrderByDescending(t => t.LaunchDate.Day).Take(3).ToList(),
                Portfolios = x.Portfolios.Select(t => new ViewModel.Portfolio.PortfolioViewModel()
                {
                    id = t.id,
                    Desc = t.Desc,
                    Heading = t.Heading,
                    PhotoPath = t.PhotoPath,
                    created_At = t.created_At,
                    Ser_Id = t.Ser_Id,
                }).OrderByDescending(t => t.created_At.Year)
                .OrderByDescending(t => t.created_At.Month)
                .OrderByDescending(t => t.created_At.Day).Take(4).ToList()
            }).FirstOrDefault();

            data.Reviews = context.Reviews.Where(x=> x.ReviewStaus==Models.Review.ReviewStatus.Approve).Select(x => new ReviewViewModel()
            {
                id = x.id,
                Rating = x.Rating,
                Desc = x.Desc,
                created_At = x.created_At,
                UserName = x.User.UserName,
                UserPhoto = x.User.Photopath,
            }).OrderByDescending(t => t.created_At)
               .Take(5).ToList();
            return data;
        }
        public Dashboard Dashboard()
        {
            throw new NotImplementedException();
        }

        public List<CustomQuoteViewModel> GetAllCustomQuotes()
        {
            var data = context.CustomQuotes.Select(x => new CustomQuoteViewModel()
            {
                Country = x.Country,
                Date = x.Date,
                Email = x.Email,
                id = x.id,
                Message = x.Message,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber
            }).OrderByDescending(x => x.Date.Year).ThenByDescending(x => x.Date.Month).ThenByDescending(x => x.Date.Day).ToList();
            return data;
        }
    }
}
