using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Promos;
using SoftwareHouseWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class PromoRepository : IPromoRepository
    {
        public ApplicationDbContext context;
        public PromoRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public int AddPromo(PromoViewModel model)
        {
            Promo p = new Promo()
            {
                Ser_id= model.Ser_id,
                PromoCode = model.PromoCode,
                PromoDiscount = model.PromoDiscount
            };
            context.Promos.Add(p);
            context.SaveChanges();
            return p.id;
        }

        public bool delete(int id)
        {
            var result = context.Promos.FirstOrDefault(u => u.id == id);
            if (result != null)
            {
                context.Entry(result).State= Microsoft.EntityFrameworkCore.EntityState.Deleted;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public PromoViewModel GetPromo(int id)
        {
            var data = context.Promos.Select(x => new PromoViewModel()
            {
                id = x.id,
                Ser_id = x.Ser_id,
                ServiceName = x.Services.ServiceName,
                PromoCode = x.PromoCode,
                PromoDiscount = x.PromoDiscount
            }).FirstOrDefault(x => x.id == id);
            return data;
        }
        public List<PromoViewModel> GetPromosOfService(int Ser_id)
        {
            var data = context.Promos.Where(x=> x.Ser_id== Ser_id).Select(x => new PromoViewModel()
            {
                id = x.id,
                Ser_id = x.Ser_id,
                ServiceName = x.Services.ServiceName,
                PromoCode = x.PromoCode,
                PromoDiscount = x.PromoDiscount
            }).ToList();
            return data;
        }
        public PromoViewModel GetInitialPromo(int Ser_id)
        {
            var data = context.Promos.Where(x => x.Ser_id == Ser_id).Select(x => new PromoViewModel()
            {
                id = x.id,
                Ser_id = x.Ser_id,
                PromoCode = x.PromoCode,
                PromoDiscount = x.PromoDiscount
            }).FirstOrDefault(x=> x.PromoCode.ToLower().Contains("initial20"));
            return data;
        }
        public bool GetPromoByCode(string code)
        {
           return context.Promos.Any(x => x.PromoCode == code);
        }

        public List<PromoViewModel> ListPromo()
        {
            var data = context.Promos.Select(x => new PromoViewModel()
            {
                id = x.id,
                Ser_id = x.Ser_id,
                ServiceName = x.Services.ServiceName,
                PromoCode = x.PromoCode,
                PromoDiscount = x.PromoDiscount
            }).ToList();
            return data;
        }

        public int Update(int id, PromoViewModel model)
        {
            Promo p = new Promo()
            {
                id = id,
                Ser_id= model.Ser_id,
                PromoCode = model.PromoCode,
                PromoDiscount = model.PromoDiscount
            };
            context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return p.id;
        }
    }
}
