using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Customer;
using SoftwareHouseWeb.Data.Models.Orders;
using SoftwareHouseWeb.Security.Tokens;
using SoftwareHouseWeb.Services.Email;
using SoftwareHouseWeb.ViewModel.OrderViewModel;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public ApplicationDbContext context;
        private readonly IDataProtector protector;
        public UserManager<ApplicationUser> UserManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<OrderRepository> logger;
        private readonly IEmailSender _emailSender;
        utilities util;
        public OrderRepository(ApplicationDbContext _context, IDataProtectionProvider dataProtectionProvider, DataProctectionPurposeString dataProtectionPurposeStrings, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> userManager, ILogger<OrderRepository> _logger, IEmailSender emailSender)
        {
            context = _context;
            util = new utilities(context);
            this.protector = dataProtectionProvider.CreateProtector(
           dataProtectionPurposeStrings.PurposeString);
            httpContextAccessor = _httpContextAccessor;
            UserManager = userManager;
            logger = _logger;
            _emailSender = emailSender;

        }
        public List<OrderViewModel> Details()
        {
            var orders = context.Orders.Select(x => new OrderViewModel()
            {
                order_id = x.order_id,
                Date = x.Date,
                PaymentStatus = x.PaymentStatus,
                OrderStatus = x.OrderStatus,
                PaymentMethod = x.PaymentMethod,
                Message = x.Message,
                OrderPackages = x.OrderPackages.Select(y => new OrderPackagesViewModel()
                {
                    id = y.id,
                    PkgName = y.Packages.PkgName,
                    pkg_Id = y.pkg_Id,
                    price = y.price,
                    Quantity = y.Quantity
                }).ToList(),
                TotalAmount = x.TotalAmount,
                cus_id = x.cus_id,
                cus_name = x.Cus_model.cus_name,
                Email = x.Cus_model.Email,
                cus_phone = x.Cus_model.PhoneNo,
                Country = x.Cus_model.Country
            }).ToList();

            return orders;
        }
        //initial20 hardcoded
        public async Task<int> Create(OrderViewModel model)
        {
            //Tranform Packages into Order Packages
            //model.OrderPackages = model.Packages.SelectMany(x => x.Packages).Where(x => x.is_selected == true).Select(x => new OrderPackagesViewModel()
            // {
            //     PkgName= x.PkgName,
            //     pkg_Id= x.id,
            //     price= x.DiscountPercent/100*x.TotalPrice,
            //     Quantity= x.Quantity,
            //     Ser_Id= x.Ser_Id
            // }).ToList();

            if (model.OrderPackages.Any(x => x.Quantity <= 0) || model.OrderPackages.Count <= 0) return -3;
            double PromoDiscount = 0; //To Subtract PromoDiscount Price
            Task<IdentityResult> result1 = null;
            var user = await UserManager.GetUserAsync(httpContextAccessor.HttpContext.User);
            var prices = util.Price(model.OrderPackages.Select(x => x.pkg_Id).ToArray());
            if (model.PromoCode != null)
            {

                if (model.PromoCode.ToLower().Contains("initial"))
                {
                    if (user.InitialPromoUsed == false)
                    {
                        user.InitialPromoUsed = true;
                    }
                    else
                    {
                        return -2;
                    }

                }
                var Promo = util.CheckPromo(model.PromoCode);
                double discount = Promo.Item2;
                if (discount == 0) return -3;
                double TotalPriceOfService = prices.Where(x => x.Item1 == Promo.Item1).Sum(x => x.Item4);
                PromoDiscount = ((discount / 100) * TotalPriceOfService); // DiscountedPrice
            }
            Models.Orders.Order order = new Models.Orders.Order()
            {
                Date = DateTime.Now,
                PaymentStatus = PaymentStatus.Unpaid,
                OrderStatus = OrderStatus.Pending,
                PaymentMethod = model.PaymentMethod,
                Message = model.Message,
                OrderPackages = model.OrderPackages.Select(x => new OrderPackages()
                {
                    pkg_Id = x.pkg_Id,
                    Quantity = x.Quantity,
                    price = prices.FirstOrDefault(p => p.Item2 == x.pkg_Id).Item4 * x.Quantity,
                }).ToList(),
            };
            order.TotalAmount = order.OrderPackages.Sum(x => x.price) - PromoDiscount;

            #region Customer
            var result = context.Customers.Select(x => new { x.cus_id, x.User_Id }).FirstOrDefault(x => x.User_Id == user.Id);
            if (result != null)
            {
                order.cus_id = result.cus_id;
            }
            else
            {
                order.Cus_model = new Models.Customer.Customer()
                {
                    cus_name = model.cus_name,
                    PhoneNo = model.cus_phone,
                    Email = model.Email,
                    Country = model.Country,
                    User_Id = user.Id,
                };
            }
            #endregion

            context.Orders.Add(order);
            context.SaveChanges();
            model.order_id = order.order_id;
            model.OrderStatus = order.OrderStatus;
            model.PaymentStatus = order.PaymentStatus;
            string str = await ViewToStringRenderer.RenderViewToStringAsync(httpContextAccessor.HttpContext.RequestServices, $"~/Views/Template/OrderConfirmation.cshtml", model);
            await _emailSender.SendEmailAsync(user.Email, "Order Confirmation", str);

            user.User_Communication = new UsersCommunication()
            {
                FirstPreferences = model.FirstPreferences,
                SecondPreferences = model.SecondPreferences,
                FirstPreferedTimeStart= model.FirstPreferedTimeStart,
                FirstPreferedTimeEnd = model.FirstPreferedTimeEnd,
                SecondPreferedTimeStart= model.SecondPreferedTimeStart,
                SecondPreferedTimeEnd= model.SecondPreferedTimeEnd
            };

            if (user.user_communication != null)
            {
                user.User_Communication.id = (int)user.user_communication;
            }
            IdentityResult r = await UserManager.UpdateAsync(user);
            if (!r.Succeeded)
            {
                logger.LogError($"Unable to Update User's({user.Email}) InitialPromoUsed! Update it Manually. ID: {user.Id}");
            }


            return order.order_id;
        }

        public bool UpdateStatus(int order_id, OrderStatus status)
        {
            var order = context.Orders
            .FirstOrDefault(x => x.order_id == order_id);
            if (order != null)
            {

                if (order.OrderStatus == status) return true;
                if (status == OrderStatus.Pending || status == OrderStatus.Processing && order.OrderStatus == OrderStatus.Completed)
                {
                    return false;
                }
                else
                {
                    order.OrderStatus = status;
                    if (status == OrderStatus.Completed)
                    {
                        order.EndDate = DateTime.Now;
                    }
                }
                context.Orders.Update(order);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public OrderViewModel OrderTeam(int order_id)
        {
            var orders = context.Orders.Select(x => new OrderViewModel()
            {
                order_id = x.order_id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                OrderTeam = x.OrderTeam.Select(y => new OrderTeamViewModel()
                {
                    EmployeeName = y.Employee.Name,
                    emp_id = y.emp_id,
                    Position = y.Employee.Position,
                    order_id = y.order_id,
                    id = y.id
                }).ToList()
            }).FirstOrDefault(x => x.order_id == order_id);

            return orders;
        }

        public bool UpdateTeam(SetupTeam model)
        {
            var order = context.Orders.Find(model.order_id);
            if (order != null)
            {
                order.StartDate = model.StartDate;
                order.EndDate = model.EndDate;
                order.OrderStatus = OrderStatus.Processing;
                if (order.OrderTeam != null)
                {
                    context.OrderTeam.RemoveRange(order.OrderTeam);
                }
                foreach (var member in model.Members.Where(x => x.IsSelected == true))
                {
                    OrderTeam o = new OrderTeam()
                    {
                        emp_id = member.Employee_id,
                        order_id = member.order_id
                    };
                    context.OrderTeam.Add(o);
                }
                context.Orders.Update(order);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public OrderViewModel Detail(int order_id)
        {
            var orders = context.Orders.Select(x => new OrderViewModel()
            {
                order_id = x.order_id,
                Date = x.Date,
                PaymentStatus = x.PaymentStatus,
                OrderStatus = x.OrderStatus,
                PaymentMethod = x.PaymentMethod,
                Message = x.Message,
                OrderPackages = x.OrderPackages.Select(y => new OrderPackagesViewModel()
                {
                    id = y.id,
                    PkgName = y.Packages.PkgName,
                    Ser_Id = y.Packages.Ser_Id,
                    pkg_Id = y.pkg_Id,
                    price = y.price,
                    Quantity = y.Quantity
                }).ToList(),
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TotalAmount = x.TotalAmount,
                cus_id = x.cus_id,
                cus_name = x.Cus_model.cus_name,
                Email = x.Cus_model.Email,
                cus_phone = x.Cus_model.PhoneNo,
                Country = x.Cus_model.Country,
            }).FirstOrDefault(x => x.order_id == order_id);

            return orders;
        }

        public List<OrderViewModel> UserOrders(string userId, bool findstatus)
        {
            var order = context.Orders.Where(x => x.Cus_model.User_Id == userId).Select(x => new OrderViewModel()
            {
                cus_name = x.Cus_model.cus_name,
                cus_id = x.cus_id,
                Email = x.Cus_model.Email,
                Country = x.Cus_model.Country,
                cus_phone = x.Cus_model.PhoneNo,
                Date = x.Date,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                Message = x.Message,
                order_id = x.order_id,
                OrderStatus = x.OrderStatus,
                PaymentMethod = x.PaymentMethod,
                PaymentStatus = x.PaymentStatus,
                TotalAmount = x.TotalAmount,
                OrderPackages = x.OrderPackages.Select(y => new OrderPackagesViewModel()
                {
                    id = y.id,
                    order_id = y.order_id,
                    PkgName = y.Packages.PkgName,
                    price = y.price,
                    pkg_Id = y.pkg_Id,
                    Quantity = y.Quantity
                }).ToList(),
                OrderTeam = x.OrderTeam.Select(y => new OrderTeamViewModel()
                {
                    emp_id = y.emp_id,
                    EmployeeName = y.Employee.Name,
                    order_id = y.order_id,
                    id = y.id,
                    Position = y.Employee.Position
                }).ToList()

            });
            if (findstatus) order = order.Where(x => x.PaymentStatus == PaymentStatus.Unpaid);
            return order.ToList();
        }

        public List<OrderViewModel> Orders(string columnName, string searchName)
        {
            var filterResults = PredicateBuilder.ContainsPredicate<Models.Orders.Order>(columnName, searchName);
            var orders = context.Orders.Where(filterResults).Select(x => new OrderViewModel()
            {
                order_id = x.order_id,
                Date = x.Date,
                PaymentStatus = x.PaymentStatus,
                OrderStatus = x.OrderStatus,
                PaymentMethod = x.PaymentMethod,
                Message = x.Message,
                OrderPackages = x.OrderPackages.Select(y => new OrderPackagesViewModel()
                {
                    id = y.id,
                    PkgName = y.Packages.PkgName,
                    pkg_Id = y.pkg_Id,
                    price = y.price,
                    Quantity = y.Quantity
                }).ToList(),
                TotalAmount = x.TotalAmount,
                cus_id = x.cus_id,
                cus_name = x.Cus_model.cus_name,
                Email = x.Cus_model.Email,
                cus_phone = x.Cus_model.PhoneNo,
                Country = x.Cus_model.Country
            }).ToList();

            return orders;
        }

        public List<OrderViewModel> OrdersWithStatus(OrderStatus status)
        {
            var orders = context.Orders.Where(x => x.OrderStatus == status).Select(x => new OrderViewModel()
            {
                order_id = x.order_id,
                Date = x.Date,
                PaymentStatus = x.PaymentStatus,
                OrderStatus = x.OrderStatus,
                PaymentMethod = x.PaymentMethod,
                Message = x.Message,
                OrderPackages = x.OrderPackages.Select(y => new OrderPackagesViewModel()
                {
                    id = y.id,
                    PkgName = y.Packages.PkgName,
                    pkg_Id = y.pkg_Id,
                    price = y.price,
                    Quantity = y.Quantity
                }).ToList(),
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                OrderTeam = x.OrderTeam.Select(y => new OrderTeamViewModel()
                {
                    emp_id = y.emp_id,
                    EmployeeName = y.Employee.Name,
                    order_id = y.order_id,
                    Position = y.Employee.Position,
                    id = y.id
                }).ToList(),
                TotalAmount = x.TotalAmount,
                cus_id = x.cus_id,
                cus_name = x.Cus_model.cus_name,
                Email = x.Cus_model.Email,
                cus_phone = x.Cus_model.PhoneNo,
                Country = x.Cus_model.Country
            }).ToList();

            return orders;
        }

        //-1 Payment gatway fail
        //-2 not found
        public int Charge(PaymentGatwayViewModel model)
        {
            var order = context.Orders.FirstOrDefault(x => x.order_id == model.order_id);
            if (order != null)
            {
                if (model.PaymentMethod == PaymentMethods.Stripe)
                {
                    if (model.StripeToken == null) return -1;
                    double a = order.TotalAmount * 0.63;
                    //double per = 2.9 / 100 * a;
                    var Options = new ChargeCreateOptions
                    {
                        Amount = Convert.ToInt32(a),
                        Currency = "usd",
                        Description = "User Emal: " + model.Email,
                        Source = model.StripeToken
                    };
                    var service = new ChargeService();
                    Charge charge = service.Create(Options);
                    if (charge.BalanceTransactionId == null || charge.Status.ToLower() != "succeeded")
                    {
                        return -1;
                    }
                    order.ChargeID = charge.Id;
                    order.PaymentStatus = PaymentStatus.Paid;
                    context.Orders.Update(order);
                }
                else if (model.PaymentMethod == PaymentMethods.Cash)
                {
                    order.ChargeID = null;
                    order.PaymentStatus = PaymentStatus.Paid;
                    context.Orders.Update(order);
                }
                // further application specific code goes here
                context.SaveChanges();
                return order.order_id;
            }
            return -2;
        }

        public bool CancelOrder(int id)
        {
            var result = context.Orders.FirstOrDefault(u => u.order_id == id);
            if (result != null)
            {
                if (result.PaymentStatus == PaymentStatus.Unpaid && (int)result.OrderStatus <= 2)
                {
                    result.OrderStatus = OrderStatus.Cancelled;
                    context.Entry(result).Property("OrderStatus").IsModified = true;
                    context.SaveChanges();
                    return true;
                }
                else return false;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var result = context.Orders.FirstOrDefault(u => u.order_id == id);
            if (result != null)
            {
                if (result.PaymentStatus == PaymentStatus.Unpaid)
                {
                    context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        //-1 not found
        //-2 updating paid order
        //-3 for Quantity negative or All Packages null
        public int Update(OrderViewModel model)
        {
            //Tranform Packages into Order Packages
            //model.OrderPackages = model.ServicesPackages.SelectMany(x => x.Packages).Where(x => x.is_selected == true).Select(x => new OrderPackagesViewModel()
            //{
            //    PkgName = x.PkgName,
            //    pkg_Id = x.id,
            //    price = x.DiscountPercent / 100 * x.TotalPrice,
            //    Quantity = x.Quantity,
            //    Ser_Id = x.Ser_Id
            //}).ToList();


            double TotalPrice = 0;
            var ListForPrice = new List<Tuple<int, int, double, double>>();
            //List of items which Quantity Increases or decreses
            //Quantity of Item Should Update in DB
            var modifiedItemsAdded = new List<OrderPackagesViewModel>();
            var modifiedItemsRemoved = new List<OrderPackagesViewModel>();

            var data = context.Orders.Where(x => x.order_id == model.order_id).FirstOrDefault();
            //Errors
            if (data == null) return -1;
            if (data.PaymentStatus != PaymentStatus.Unpaid || data.OrderStatus != OrderStatus.Pending) return -2;
            if (model.OrderPackages.Any(x => x.Quantity <= 0) || model.OrderPackages.Count <= 0) return -3;
            var ProductsList = context.OrderPackages.Where(x => x.order_id == model.order_id).Select(x => new OrderPackagesViewModel()
            {
                id = x.id,
                Quantity = x.Quantity,
                pkg_Id = x.pkg_Id,
                order_id = x.order_id,
                price = x.price,
            }).ToList();
            model.OrderPackages.ForEach(x => x.order_id = data.order_id);

            //Quantity of Item Should Update in DB as well as add it to as Order with status completed
            var NewItems = model.OrderPackages.Except(model.OrderPackages.Where(o => ProductsList.Select(s => s.pkg_Id).ToList().Contains(o.pkg_Id)));
            //Quantity of Item Should Update in DB as well as remove it to from Order Table.
            var RemoveItems = ProductsList.Except(ProductsList.Where(o => model.OrderPackages.Select(s => s.pkg_Id).ToList().Contains(o.pkg_Id)));

            ListForPrice = util.Price(model.OrderPackages.Select(x => x.pkg_Id).ToArray());

            //Working For Checking Quantity Changes. If changes update OrderPackages table with Quantity and Prices
            foreach (var p in model.OrderPackages)
            {
                var found = ProductsList.FirstOrDefault(y => y.pkg_Id == p.pkg_Id);
                if (found != null)
                {
                    if (found.Quantity > p.Quantity)
                    {
                        double PriceOfEach = ListForPrice.FirstOrDefault(x => x.Item2 == found.pkg_Id).Item4;
                        TotalPrice -= PriceOfEach * (found.Quantity - p.Quantity);
                        found.Quantity = p.Quantity;
                        found.price = PriceOfEach * p.Quantity;
                        modifiedItemsRemoved.Add(found);
                    }
                    else if (found.Quantity < p.Quantity)
                    {
                        double PriceOfEach = ListForPrice.FirstOrDefault(x => x.Item2 == found.pkg_Id).Item4;
                        TotalPrice += PriceOfEach * (p.Quantity - found.Quantity);
                        found.Quantity = p.Quantity;
                        found.price = PriceOfEach * p.Quantity;
                        modifiedItemsAdded.Add(found);
                    }
                }
            }

            if (modifiedItemsAdded.Count != 0)
            {
                var dataModel = modifiedItemsAdded.Select(x => new OrderPackages()
                {
                    id = x.id,
                    pkg_Id = x.pkg_Id,
                    price = x.price,
                    Quantity = x.Quantity,
                    order_id = x.order_id
                }).ToList();
                context.OrderPackages.UpdateRange(dataModel);
            }
            //Updating Product Table if any exisitng item quantity changes(removed)
            if (modifiedItemsRemoved.Count != 0)
            {
                var dataModel = modifiedItemsRemoved.Select(x => new OrderPackages()
                {
                    id = x.id,
                    pkg_Id = x.pkg_Id,
                    price = x.price,
                    Quantity = x.Quantity,
                    order_id = x.order_id
                }).ToList();
                context.OrderPackages.UpdateRange(dataModel);
            }
            //Add New Items To Product Table if added
            if (NewItems.Count() != 0)
            {
                var dataModel = NewItems.Select(x => new OrderPackages()
                {
                    pkg_Id = x.pkg_Id,
                    //Phoneid = x.Phoneid,
                    price = ListForPrice.FirstOrDefault(p => p.Item2 == x.pkg_Id).Item4 * x.Quantity,
                    Quantity = x.Quantity,
                    order_id = x.order_id
                }).ToList();
                TotalPrice += dataModel.Sum(x => x.price);
                context.OrderPackages.AddRange(dataModel);
            }
            //Removing Exisiting Items from Product Table if removed
            if (RemoveItems.Count() != 0)
            {
                var dataModel = RemoveItems.Select(x => new OrderPackages()
                {
                    id = x.id,
                    pkg_Id = x.pkg_Id,
                    order_id = x.order_id,
                    Quantity = x.Quantity
                });
                TotalPrice -= RemoveItems.Sum(x => x.price);
                context.OrderPackages.RemoveRange(dataModel);
            }

            //Updating Fields
            data.Message = model.Message;
            data.PaymentMethod = model.PaymentMethod;
            data.TotalAmount = data.TotalAmount + TotalPrice;
            data.Cus_model.cus_name = model.cus_name;
            data.Cus_model.PhoneNo = model.cus_phone;
            data.Cus_model.Email = model.Email;
            data.Cus_model.Country = model.Country;

            context.Orders.Update(data);
            context.SaveChanges();
            return data.order_id;
        }
    }
}
