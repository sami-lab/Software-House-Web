using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SoftwareHouseWeb.Security.Tokens;
using SoftwareHouseWeb.ViewModel.PackagesViewModel;
using SoftwareHouseWeb.ViewModel.ServicesViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data
{
    public class utilities
    {
        public ApplicationDbContext context;
        private readonly IDataProtector protector;
        private readonly IHostingEnvironment hostingEnvironment;
        public utilities(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        public utilities(ApplicationDbContext _context)
        {
            context = _context;
        }
        public utilities(ApplicationDbContext _context, IHostingEnvironment hostingEnvironment)
        {
            context = _context;
            this.hostingEnvironment = hostingEnvironment;
        }
        public utilities(ApplicationDbContext _context, IDataProtectionProvider dataProtectionProvider, DataProctectionPurposeString dataProtectionPurposeStrings)
        {
            this.protector = dataProtectionProvider.CreateProtector(
           dataProtectionPurposeStrings.PurposeString);
        }

        public string ProcessPhotoproperty(IFormFile Photo,string InnerFolder)
        {
            string uniqueFileName = null;
            if (Photo != null)
            {
                string uploadedfile = Path.Combine(hostingEnvironment.WebRootPath, "Image",InnerFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filepath = Path.Combine(uploadedfile, uniqueFileName);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    Photo.CopyTo(filestream);
                }
            }

            return uniqueFileName;
        }

        public List<PackagesViewModel> GetPackages()
        {
            var result = new List<PackagesViewModel>()
            {
                new PackagesViewModel()
                {
                     id = 1,
                     Desc = "Package1,Package2,Package3,Package4,Package5,Package6,Package7,Package8,Package9",
                     TotalPrice = 200,
                     Ser_Name ="Web Design",
                     Ser_Id =1,
                     DiscountPercent =10,
                     Heading = "",
                     LaunchDate = DateTime.Now,
                     PhotoPath = null,
                     PkgName = "BASIC",
                },
                new PackagesViewModel()
                {
                     id = 2,
                     Desc = "Package1,Package2,Package3,Package4,Package5,Package6,Package7,Package8,Package9",
                     TotalPrice = 300,
                     Ser_Name ="Web Development",
                     Ser_Id =2,
                     DiscountPercent =10,
                     Heading = "",
                     LaunchDate = DateTime.Now,
                     PhotoPath = null,
                     PkgName = "STARTUP",
                },
                new PackagesViewModel()
                {
                     id = 3,
                     Desc = "Package1,Package2,Package3,Package4,Package5,Package6,Package7,Package8,Package9",
                     TotalPrice = 200,
                     Ser_Name ="Logo Design",
                     Ser_Id =3,
                     DiscountPercent =10,
                     Heading = "",
                     LaunchDate = DateTime.Now,
                     PhotoPath = null,
                     PkgName = "Pro",
                }
            };
            return result;
        }

        public List<ServicesViewModel> GetAllServices()
        {
            var result = context.Services.Where(x => x.isActive == true).Select(x => new ServicesViewModel()
            {
                id = x.id,
                ServiceName = x.ServiceName,
            }).ToList();
            return result;
        }

        public List<PackagesViewModel> GetPackagesOfService(int Ser_Id)
        {
            var result = context.Packages.Where(x => x.isActive == true && x.Ser_Id==Ser_Id).Select(x => new PackagesViewModel()
            {
                Encrypted_id = protector.Protect(x.id.ToString()),
                PkgName = x.PkgName,
            }).OrderBy(x=> x.LaunchDate).ToList();
            return result;
        }

        //Calculate Price of Packages(return serviceId,pkgId,price,DiscountPrice)
        public List<Tuple<int,int, double,double>> Price(int[] pkg_ids)
        {
            var result = context.Packages.Select(x => new {x.Ser_Id, x.id, x.TotalPrice,x.DiscountPercent }).Where(x => pkg_ids.Contains(x.id)).Select(c => new Tuple<int,int, double,double>(c.Ser_Id,c.id,c.TotalPrice, c.TotalPrice - ((c.DiscountPercent / 100) * c.TotalPrice))).ToList();
            return result;
        }

        //It return ServiceId and DiscountPercent
        public Tuple<int,double> CheckPromo(string promcode)
        {
            var promo = context.Promos.FirstOrDefault(x => x.PromoCode == promcode);
            if(promo != null) return new Tuple<int,double> (promo.Ser_id, promo.PromoDiscount);
            return  new Tuple<int, double> (0,0);
        }
    }
}
