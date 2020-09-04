using Microsoft.AspNetCore.Hosting;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Services;
using SoftwareHouseWeb.ViewModel.ServicesViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class ServicesRepository : IServicesRepository
    {
        public ApplicationDbContext context;
        IHostingEnvironment hostingEnvironment;
        utilities util;
        public ServicesRepository(ApplicationDbContext _context, IHostingEnvironment _hostingEnvironment)
        {
            context = _context;
            hostingEnvironment = _hostingEnvironment;
            util = new utilities(_hostingEnvironment);
        }
        public int addService(ServicesViewModel c)
        {
            string uniqueFileName = util.ProcessPhotoproperty(c.Photo, "Services");
            OurServices model = new OurServices()
            {
                ServiceName= c.ServiceName,
                IconPath= uniqueFileName,
                 isActive =true
            };
            context.Services.Add(model);
            context.SaveChanges();
            return model.id;
        }

        public bool delete(int id)
        {
            var result = context.Services.FirstOrDefault(u => u.id == id);
            if (result != null)
            {
                result.isActive = false;
                context.Entry(result).Property("isActive").IsModified = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public ServicesViewModel GetDetail(int id)
        {
            var result = context.Services.Where(x=> x.isActive==true).Select(x => new ServicesViewModel()
            {
               id= x.id,
               ServiceName= x.ServiceName,
               IconPath= x.IconPath
            }).FirstOrDefault(x => x.id == id);
            return result;
        }

        public List<ServicesViewModel> GetDetails()
        {
            var result = context.Services.Where(x => x.isActive == true).Select(x => new ViewModel.ServicesViewModel.ServicesViewModel()
            {
                id = x.id,
                ServiceName = x.ServiceName,
                IconPath= x.IconPath
            }).ToList();
            return result;
        }

        public int Update(int id, ServicesViewModel emp)
        {
            var data = context.Services.Find(id);
            if (data != null)
            {
                string uniqueFileName = "";
                if (emp.Photo != null)
                {
                    if (data.IconPath != null)
                    {
                        string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Image", "Services", data.IconPath);
                        System.IO.File.Delete(filepath);
                    }
                    uniqueFileName = util.ProcessPhotoproperty(emp.Photo, "Services");
                }
                else
                {
                    uniqueFileName = data.IconPath;
                }

                data.id = id;
                data.ServiceName = emp.ServiceName;
                data.IconPath = uniqueFileName;
                data.isActive = true;
                context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return data.id; 
            }
            return -1;
        }

       
    }
}
