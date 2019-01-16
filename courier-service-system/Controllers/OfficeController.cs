using CSSEntity;
using CSSService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace courier_service_system.Controllers
{
    public class OfficeController : Controller
    {

        // GET: Office
        OfficeService officeService = new OfficeService();
        EmployeeService employeeService = new EmployeeService();
        public ActionResult Index(string id)
        {
            Session["CurrentOffice"] = null;
            Session.Remove("CurrentOffice");
            if (id == null)
            {
                if (Session["officeDivision"] == null)
                {
                    Session["officeDivision"] = "Barishal";
                }
            }
            else
            {
                Session["officeDivision"] = id;
            }

            return View(officeService.GetAllForOffice(Session["officeDivision"].ToString()));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(OfficeViewModel ofc)
        {
            if (ModelState.IsValid)
            {
                Office newOffice = new Office();
                string division = ofc.Division;
                int count = officeService.getRowCount() + 1;
                string offcieid = division[0].ToString() + division[1].ToString() + division[2].ToString() + "-" + ofc.Area + "-" + count;
                newOffice.OfficeId = offcieid;
                newOffice.Location = "House : " + ofc.House + ", Road : " + ofc.Road + ", " + ofc.Area + ", " + division;
                newOffice.OfficialNumber = ofc.OfficialNumber;
                officeService.Insert(newOffice);
                return RedirectToAction("Index", "Office");
            }
            return View(ofc);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Office ofc = officeService.Get(id);
            return View(ofc);
        }
        [HttpPost]
        public ActionResult Edit(Office ofc)
        {
            officeService.Update(ofc);
            return RedirectToAction("Index", "Office");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            Office ofc = officeService.Get(id);
            return View(ofc);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            officeService.Delete(id);
            return RedirectToAction("Index", "Office");
        }
        [HttpGet]
        public ActionResult ShowEmployees(string id)
        {
            Session["CurrentState"] = null;
            Session.Remove("CurrentState");
            Session["officeId"] = id;
            string division = id[0].ToString() + id[1].ToString() + id[2].ToString();
            List<Employee> emplist = employeeService.GetEmployeesForOffice(division);
            return View(emplist);
        }
        [HttpGet]
        public ActionResult AddNewEmployee()
        {
            Session["CurrentState"] = "Office";
            return View();
        }
        [HttpPost]
        public ActionResult AddNewEmployee(Employee employee)
        {
            string Position = Request["Position"].ToString();
            employee.RegistrationTime = DateTime.Now;
            employee.Password = employeeService.generatePass();
            employee.Id = employeeService.generateId(Position, employee.RegistrationTime.ToString());
            employee.OfficeId = Session["officeId"].ToString();
            employee.CurrentStatus = "Active";
            employee.CurrentLocation = Session["officeId"].ToString();
            employeeService.Insert(employee);
            Session["CurrentState"] = "Office";
            return RedirectToAction("Confirmation", "SendMailer", new { id = employee.Id });
        }
        public ActionResult Result(string id)
        {
            Employee emp = employeeService.Get(id);
            return View(emp);
        }
        [HttpGet]
        public ActionResult EditEmployee(string id)
        {
            Employee emp = employeeService.Get(id);
            OfficeService officeRepository = new OfficeService();
            List<SelectListItem> officeList = new List<SelectListItem>();
            foreach (Office office in officeRepository.GetAll())
            {
                SelectListItem option = new SelectListItem();
                option.Text = office.OfficeId;
                option.Value = office.OfficeId;
                officeList.Add(option);
            }
            
            ViewBag.Offices = officeList;
            return View(emp);
        }
        [HttpPost]
        public ActionResult EditEmployee(Employee employeeUpdate)
        {
            employeeService.Update(employeeUpdate);
            return RedirectToAction("ShowEmployees", "Office", new { id = employeeUpdate.OfficeId});
        }
        [HttpGet]
        public ActionResult EmployeeDetails(string id)
        {
            Employee emp = employeeService.Get(id);
            return View(emp);
        }
        
        [HttpGet]
        public ActionResult DeleteEmployee(string id)
        {
            Employee emp= employeeService.Get(id);
            return View(emp);
        }
        [HttpPost,ActionName("DeleteEmployee")]
        public ActionResult ConfirmDeleteEmployee(string id)
        {
            employeeService.Delete(id);
            return RedirectToAction("ShowEmployees", "Office", new { id = Session["officeId"]});
        }
    }
}