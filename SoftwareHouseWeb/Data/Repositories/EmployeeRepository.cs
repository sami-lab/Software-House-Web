using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Models.Employee;
using SoftwareHouseWeb.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public ApplicationDbContext context;
        public EmployeeRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public int AddEmployee(RegisterEmployeeViewModel model)
        {
            var user = new Employee()
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Position = model.Position,
                Time = model.Time,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Skills = model.Skill1 + "," + model.Skill2 + "," + model.Skill3 + "," + model.Skill4,
                Salary = model.Salary,
                age = model.age,
                is_active = true,
            };

            context.Employees.Add(user);
            context.SaveChanges();
            return user.Employee_id;
        }

        public List<RegisterEmployeeViewModel> ListEmployees()
        {
            var employees = context.Employees.Where(x => x.is_active == true).Select(x => new RegisterEmployeeViewModel()
            {
                Employee_id = x.Employee_id,
                Email = x.Email,
                age = x.age,
                Name = x.Name,
                EndDate = x.EndDate,
                PhoneNumber = x.PhoneNumber,
                Position = x.Position,
                Skills = x.Skills,
                StartDate = x.StartDate,
                Salary = x.Salary,
                Time = x.Time
            }).ToList();
            return employees;
        }

        public List<RegisterEmployeeViewModel> ListInterns()
        {
            var employees = context.Employees.Where(x => x.is_active == true).Select(x => new RegisterEmployeeViewModel()
            {
                Employee_id = x.Employee_id,
                Email = x.Email,
                age = x.age,
                Name = x.Name,
                EndDate = x.EndDate,
                PhoneNumber = x.PhoneNumber,
                Position = x.Position,
                Skills = x.Skills,
                StartDate = x.StartDate,
                Salary = x.Salary,
                Time = x.Time
            }).Where(x => x.Time == Time.Intern).ToList();
            return employees;
        }

        public RegisterEmployeeViewModel Employee(int Employee_id)
        {
            var employees = context.Employees.Where(x=> x.is_active ==true).Select(x => new RegisterEmployeeViewModel()
            {
                Employee_id = x.Employee_id,
                Email = x.Email,
                age = x.age,
                Name = x.Name,
                EndDate = x.EndDate,
                PhoneNumber = x.PhoneNumber,
                Position = x.Position,
                Skills = x.Skills,
                StartDate = x.StartDate,
                Salary = x.Salary,
                Time = x.Time
            }).FirstOrDefault(x => x.Employee_id == Employee_id);
            return employees;
        }

        public bool delete(int Employee_id)
        {
            var result = context.Employees.FirstOrDefault(u => u.Employee_id == Employee_id);
            if (result != null)
            {
                result.is_active = false;
                result.EndDate = DateTime.Now;
                context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
