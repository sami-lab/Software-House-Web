using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Packages;
using SoftwareHouseWeb.Security.Tokens;
using SoftwareHouseWeb.ViewModel.PackagesViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class PackagesRepository : IPackagesRepository
    {
        public ApplicationDbContext context;
        IHostingEnvironment hostingEnvironment;
        private readonly IDataProtector protector;
        utilities util;
        public PackagesRepository(ApplicationDbContext _context, IHostingEnvironment _hostingEnvironment,IDataProtectionProvider dataProtectionProvider, DataProctectionPurposeString dataProtectionPurposeStrings)
        {
            context = _context;
            hostingEnvironment = _hostingEnvironment;
            this.protector = dataProtectionProvider.CreateProtector(
            dataProtectionPurposeStrings.PurposeString);
            util = new utilities(hostingEnvironment);
        }
        public string addPackage(PackagesViewModel c)
        {
            string uniqueFileName = util.ProcessPhotoproperty(c.Photo,"Packages");
            Packages model = new Packages()
            {
                Desc = c.Desc,
                DiscountPercent = c.DiscountPercent,
                Heading = c.Heading,
                isActive = true,
                LaunchDate = c.LaunchDate,
                PhotoPath = uniqueFileName,
                PkgName = c.PkgName,
                Ser_Id = c.Ser_Id,
                TotalPrice = c.TotalPrice
            };
            context.Packages.Add(model);
            context.SaveChanges();
            return protector.Protect(model.id.ToString());
        }

        public bool delete(string id)
        {
            string decryptedId = protector.Unprotect(id);
            int decryptedIntId = Convert.ToInt32(decryptedId);
            var result = context.Packages.FirstOrDefault(u => u.id == decryptedIntId);
            if (result != null)
            {
                result.isActive = false;
                context.Entry(result).Property("isActive").IsModified = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public PackagesViewModel GetDetail(string id)
        {
            string decryptedId = protector.Unprotect(id);
            int decryptedIntId = Convert.ToInt32(decryptedId);
            var result = context.Packages.Where(x => x.isActive == true).Select(x => new PackagesViewModel()
            {
                id = x.id,
                Encrypted_id= protector.Protect(x.id.ToString()),
                Desc = x.Desc,
                TotalPrice = x.TotalPrice,
                Ser_Name = x.Service_Model.ServiceName,
                Ser_Id = x.Ser_Id,
                DiscountPercent = x.DiscountPercent,
                Heading = x.Heading,
                LaunchDate = x.LaunchDate,
                PhotoPath = x.PhotoPath,
                PkgName = x.PkgName,
                Description = x.Desc.Split("`", StringSplitOptions.None)

            }).FirstOrDefault(x => x.id == decryptedIntId);
            return result;
        }

        public List<PackagesViewModel> GetDetails()
        {
            var result = context.Packages.Where(x => x.isActive == true).Select(x => new PackagesViewModel()
            {
                id = x.id,
                Encrypted_id = protector.Protect(x.id.ToString()),
                Desc = x.Desc,
                TotalPrice = x.TotalPrice,
                Ser_Name = x.Service_Model.ServiceName,
                Ser_Id = x.Ser_Id,
                DiscountPercent = x.DiscountPercent,
                Heading = x.Heading,
                LaunchDate = x.LaunchDate,
                PhotoPath = x.PhotoPath,
                PkgName = x.PkgName,
                Description = x.Desc.Split("`", StringSplitOptions.None)
            }).OrderBy(x=> x.LaunchDate).ToList();
            return result;
        }

        public List<PackagesViewModel> GetPackagesOfService(int Ser_Id)
        {
            var result = context.Packages.Where(x => x.isActive == true && x.Ser_Id == Ser_Id).Select(x => new PackagesViewModel()
            {
                id = x.id,
                Encrypted_id = protector.Protect(x.id.ToString()),
                Desc = x.Desc,
                TotalPrice = x.TotalPrice,
                Ser_Name = x.Service_Model.ServiceName,
                Ser_Id = x.Ser_Id,
                DiscountPercent = x.DiscountPercent,
                Heading = x.Heading,
                LaunchDate = x.LaunchDate,
                PhotoPath = x.PhotoPath,
                PkgName = x.PkgName,
                Description = x.Desc.Split("`", StringSplitOptions.None)
            }).ToList();
            return result;
        }

        public string Update(string id, PackagesViewModel c)
        {
            string decryptedId = protector.Unprotect(id);
            int decryptedIntId = Convert.ToInt32(decryptedId);
            var data = context.Packages.Find(decryptedIntId);
            string uniqueFileName = "";
            if (data != null)
            {
                if (c.Photo != null)
                {
                    if (data.PhotoPath != null)
                    {
                        string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Image", "Packages", data.PhotoPath);
                        System.IO.File.Delete(filepath);
                    }
                    uniqueFileName = util.ProcessPhotoproperty(c.Photo, "Packages");
                }
                else
                {
                    uniqueFileName = data.PhotoPath;
                }
               
                data.Desc = c.Desc;
                data.DiscountPercent = c.DiscountPercent;
                data.Heading = c.Heading;
                data.PhotoPath = uniqueFileName;
                data.PkgName = c.PkgName;
                data.Ser_Id = c.Ser_Id;
                data.TotalPrice = c.TotalPrice;

                context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

                return id;
            }
            return "-1";
        }
    }
}
