using Microsoft.AspNetCore.Hosting;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Portfolio;
using SoftwareHouseWeb.ViewModel;
using SoftwareHouseWeb.ViewModel.Portfolio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        public ApplicationDbContext context;
        IHostingEnvironment hostingEnvironment;
        utilities util;
        public PortfolioRepository(ApplicationDbContext _context, IHostingEnvironment _hostingEnvironment)
        {
            context = _context;
            hostingEnvironment = _hostingEnvironment;
            util = new utilities(hostingEnvironment);
        }
        public int addPortfolio(PortfolioViewModel c)
        {
            var date = DateTime.Now;
            string uniqueFileName = util.ProcessPhotoproperty(c.Photo, "Portfolio");
            Portfolio model = new Portfolio()
            {
              Ser_Id = c.Ser_Id,
              Heading= c.Heading,
              Desc =c.Desc,
              created_At= DateTime.Now,
              PhotoPath= uniqueFileName
            };
            context.Portfolio.Add(model);
            context.SaveChanges();
            return model.id;
        }

        public bool delete(int id)
        {
            var result = context.Portfolio.FirstOrDefault(u => u.id == id);
            if (result != null)
            {
                context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public PortfolioViewModel GetDetail(int id)
        {
            var result = context.Portfolio.Select(x => new PortfolioViewModel()
            {
               id = x.id,
               Desc =x.Desc,
               Heading= x.Heading,
               PhotoPath= x.PhotoPath,
               created_At= x.created_At,
               Ser_Id= x.Ser_Id,
               Ser_Name= x.Service_Model.ServiceName,
            }).FirstOrDefault(x=> x.id==id);
            return result;
        }

        public List<PortfolioViewModel> ServicePortfolio(int Ser_Id)
        {
            var result = context.Portfolio.Select(x => new PortfolioViewModel()
            {
                id = x.id,
                Desc = x.Desc,
                Heading = x.Heading,
                created_At= x.created_At,
                PhotoPath = x.PhotoPath,
                Ser_Id = x.Ser_Id,
                Ser_Name = x.Service_Model.ServiceName,
            }).Where(x => x.Ser_Id == Ser_Id).ToList();
            return result;
        }
        public List<PortfolioViewModel> GetDetails()
        {
            var result = context.Portfolio.Select(x => new PortfolioViewModel()
            {
                id = x.id,
                Desc = x.Desc,
                Heading = x.Heading,
                PhotoPath = x.PhotoPath,
                created_At= x.created_At,
                Ser_Id = x.Ser_Id,
                Ser_Name = x.Service_Model.ServiceName,
            }).ToList();
            return result;
        }

        public int Update(int id, PortfolioViewModel c)
        {
            var data = context.Portfolio.Find(id);
            string uniqueFileName = "";
            if (data != null)
            {
                if (c.Photo != null)
                {
                    if (data.PhotoPath != null)
                    {
                        string filepath = Path.Combine(hostingEnvironment.WebRootPath, "Image", "Portfolio", data.PhotoPath);
                        System.IO.File.Delete(filepath);
                    }
                    uniqueFileName = util.ProcessPhotoproperty(c.Photo, "Portfolio");
                }
                else
                {
                    uniqueFileName = data.PhotoPath;
                }

                data.Desc = c.Desc;
                data.PhotoPath = uniqueFileName;
                data.Ser_Id = c.Ser_Id;
                data.Heading = c.Heading;
                
                context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return data.id;
            }
            return -1;
        }
    }
}
