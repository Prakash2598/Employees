using Employees.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Employees.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbEntities employeeDbContext = new EmployeeDbEntities();
        public ActionResult GetAllEmployees()
        {
            var employeeLst = employeeDbContext.Employees.ToList();
            return View(employeeLst);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            Employee employee = new Employee
            {
                EmployeeName = formCollection["EmployeeName"],
                EmailId = formCollection["EmailId"],
                DateOfBirth = Convert.ToDateTime(formCollection["DateOfBirth"]),
                ExperienceLevel = formCollection["ExperienceLevel"],
                Gender = formCollection["Gender"],
                Address = formCollection["Address"]
            };

            employeeDbContext.Employees.Add(employee);
            employeeDbContext.SaveChanges();
            return RedirectToAction("GetAllEmployees");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Employee Id is required");

            Employee employee = employeeDbContext.Employees.Find(id);
            if (employee == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Employee not found");
            return View(employee);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int? id)
        {
            Employee employee = employeeDbContext.Employees.Find(id);
            employeeDbContext.Employees.Remove(employee);
            employeeDbContext.SaveChanges();
            return RedirectToAction("GetAllEmployees");
        }

        [HttpPost]
        public ActionResult GetAllEmployees(string searchText)
        {
            var employeeLst = employeeDbContext.Employees.ToList();
            if (!string.IsNullOrEmpty(searchText))
            {
                employeeLst = employeeLst.Where(emp => emp.EmployeeName.Contains(searchText) || emp.EmailId.Contains(searchText) || emp.ExperienceLevel.Contains(searchText) || emp.Address.Contains(searchText)).ToList();
            }
            return View(employeeLst);
        }
    }
}