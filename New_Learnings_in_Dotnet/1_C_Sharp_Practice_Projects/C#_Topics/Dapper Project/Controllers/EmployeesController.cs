using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DapperProject.Data;
using DapperProject.Models;
using DapperProject.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel.Design;

namespace DapperProject.Controllers
{
    public class EmployeesController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly ICompanyRepository _compRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IBonusRepository _bonRepo;

        [BindProperty]
        public Employee Employee { get; set; }

        public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo, IBonusRepository bonRepo)
        {
            _compRepo = compRepo;
            _empRepo = empRepo;
            _bonRepo = bonRepo;
        }

        private void PageLoad()
        {
            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CompanyId.ToString(),
            });
            ViewBag.CompanyList = companyList;
        }        

        public async Task<IActionResult> Index(int companyId)
        {
            ////ForEach
            //List<Employee> employees = _empRepo.GetAll();
            //foreach(Employee obj in employees)
            //{
            //    obj.Company = _compRepo.Find(obj.CompanyId);
            //}

            //// OR
            //// Linq
            //var employees = (from employee in _empRepo.GetAll()
            //                 join company in _compRepo.GetAll() on employee.CompanyId equals company.CompanyId
            //                 select new Employee()
            //                 {
            //                     EmployeeId = employee.EmployeeId,
            //                     Name = employee.Name,
            //                     Email = employee.Email,
            //                     Phone = employee.Phone,
            //                     Title = employee.Title,
            //                     CompanyId = employee.CompanyId,
            //                     Company = company,
            //                 }).ToList();

            ////OR
            /// one to one mapping in dapper (more efficient)
            var employees = _bonRepo.GetEmployeeWithCompany(companyId);
            return View(employees);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            PageLoad();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
                _empRepo.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            PageLoad();
            return View(Employee);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employee = _empRepo.Find(id.GetValueOrDefault());
            if (Employee == null)
            {
                return NotFound();
            }
            PageLoad();
            return View(Employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _empRepo.Update(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _empRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }
    }
}
