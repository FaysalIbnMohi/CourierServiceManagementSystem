using CSSEntity;
using CSSService;
//using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace courier_service_system.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        IEmployeeService Service;
        public EmployeeController()
        {
            this.Service =  Injector.Container.Resolve<IEmployeeService>();
        }
        EmployeeService employeeRepository = new EmployeeService();
        LoginReportService loginReportService = new LoginReportService();
        EmployeeService employeeService = new EmployeeService();
        RequestUpdateService requestUpdateService = new RequestUpdateService();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //[HttpPost, ActionName("Login")]
        //public ActionResult ConfirmedLogin()
        //{
        //    string id = Request["username"];
        //    string pass = Request["password"];
        //    Employee employee = employeeRepository.Get(id);
        //    if (employee != null)
        //    {
        //        if (pass == employee.Password)
        //        {
        //            Session["message"] = null;
        //            LoginReport loginReport = new LoginReport();
        //            loginReport.EmployeeId = id;
        //            Session["logTime"] = loginReport.LoginTime = DateTime.Now;
        //            Session["uname"] = employee.Name;
        //            Session["id"] = id;
        //            Session["From"] = employee.Email;
        //            Session["Position"] = employee.Position;
        //            loginReportRepository.Insert(loginReport);
        //            if (id[0] == 'A')
        //                return RedirectToAction("Dashboard", "Employee");
        //            else if (id[0] == 'D')
        //                return RedirectToAction("Dashboard", "Driver");
        //        }
        //    }
        //    Session["message"] = "Invalid User or Password";
        //    return RedirectToAction("Login");
        //}
        public ActionResult ViewProfile(string Id, string temp)
        {
            if (Session["uname"] != null)
            {
                if (temp == "1") { ViewBag.Edit = "Yes"; }
                Employee employee = employeeRepository.Get(Id);
                return View(employee);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult EditProfile(string Id, string temp)
        {
            if (Session["uname"] != null)
            {
                TempData["EditEmployeeId"] = Id;
                if (temp == "1") { ViewBag.Edit = "Yes"; }
                Employee employee = employeeRepository.Get(Id);
                OfficeService officeRepository = new OfficeService();
                List<SelectListItem> officeList = new List<SelectListItem>();
                SelectListItem init = new SelectListItem();
                init.Text = "Please Select an Option";
                init.Value = "";
                officeList.Add(init);
                foreach (Office office in officeRepository.GetAll())
                {
                    SelectListItem option = new SelectListItem();
                    option.Text = office.OfficeId;
                    option.Value = office.OfficeId;
                    officeList.Add(option);
                }
                ViewBag.Offices = officeList;
                return View(employee);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult EditProfile(FormCollection formCollection)
        {
            Employee empToUpdate = new Employee();
            empToUpdate.Id = TempData["EditEmployeeId"].ToString();
            empToUpdate.Name = Request["Name"];
            empToUpdate.Email = Request["Email"];
            empToUpdate.Salary = Convert.ToInt32(Request["Salary"]);
            empToUpdate.Birthday = Convert.ToDateTime(Request["Birthday"]);
            empToUpdate.Address = Request["Address"];
            empToUpdate.Position = Request["Position"];
            empToUpdate.PhoneNumber = Convert.ToInt32(Request["PhoneNumber"]);
            empToUpdate.NationalId = Request["NationalId"];
            empToUpdate.OfficeId = Request["OfficeId"];
            empToUpdate.JoinDate = Convert.ToDateTime(Request["JoinDate"]);
            employeeRepository.UpdateEmployee(empToUpdate);
            if (Session["id"].ToString() == empToUpdate.Id)
            {
                Session["uname"] = empToUpdate.Name;
            }
            return View("ViewProfile", empToUpdate);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["uname"] != null)
                return View();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost, ActionName("ChangePassword")]
        public ActionResult ConfirmPassword()
        {
            string Id = Session["Id"].ToString();
            Employee employee = employeeRepository.Get(Id);
            if (Request["newPassword"].Equals(Request["retypeNewPassword"]) && employee.Password.Equals(Request["Password"]))
            {
                Session["message"] = null;
                employee.Password = Request["newPassword"];
                employeeRepository.Update(employee);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["message"] = "Opps!!! Something Wrong... Try Again";
                return View("ChangePassword");
            }
        }
        public ActionResult picture()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["uname"] != null )
            {
                Employee employee = employeeRepository.Get(Session["Id"].ToString());
                ViewBag.SinceUser = Math.Round((DateTime.Now - employee.RegistrationTime).TotalDays, 2);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Registration(string id)
        {
            if (id == "1") { ViewBag.Reg = "Yes"; }
            OfficeService officeRepository = new OfficeService();
            List<SelectListItem> officeList = new List<SelectListItem>();
            SelectListItem init = new SelectListItem();
            init.Text = "Please Select an Option";
            init.Value = "";
            officeList.Add(init);
            foreach (Office office in officeRepository.GetAll())
            {
                SelectListItem option = new SelectListItem();
                option.Text = office.OfficeId;
                option.Value = office.OfficeId;
                officeList.Add(option);
            }
            ViewBag.Offices = officeList;
            return View();
        }
        [HttpPost]
        public ActionResult Registration(Employee employee)
        {
            Session["CurrentState"] = "none";
            Session["CurrentOffice"] = null;
            Session.Remove("CurrentOffice");
            string Position = Request["Position"].ToString();
            employee.RegistrationTime = DateTime.Now;
            employee.CurrentStatus = "Active";
            employee.CurrentLocation = employee.OfficeId.ToString();
            employee.Password = employeeRepository.generatePass();
            employee.Id = employeeRepository.generateId(Position, employee.RegistrationTime.ToString());
            employeeRepository.Insert(employee);
            return RedirectToAction("Confirmation", "SendMailer", new { id = employee.Id });
        }

        public ActionResult Result()
        {
            if (Session["uname"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Employees(string id)
        {
            if (Session["Id"] == null) return RedirectToAction("Index", "Home");
            if (id == "All" || id==null)
            {
                List<Employee> emp = employeeRepository.GetAll();
                return View(emp);
            }
            string division = id[0].ToString() + id[1].ToString() + id[2].ToString();
            List<Employee> employees = employeeRepository.GetEmployeesForOffice(division);
            return View(employees);
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["uname"] != null)
            {
                Employee employee = employeeRepository.Get(id);
                return View(employee);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            //employeeRepository.Delete(id);
            Employee employee = employeeRepository.Get(id);
            employee.CurrentStatus = "Left";
            employeeService.UpdateEmployee(employee);
            return RedirectToAction("Employees");
        }

        [HttpGet]
        public ActionResult Approve(string id)
        {
            if (Session["uname"] != null)
            {
                RequestUpdate requestUpdate = requestUpdateService.Get(id);
                Employee empToUpdate = new Employee();
                empToUpdate.Id = requestUpdate.Id;
                empToUpdate.Name = requestUpdate.Name;
                empToUpdate.Birthday = requestUpdate.DOB;
                empToUpdate.Address = requestUpdate.Address;
                empToUpdate.PhoneNumber = Convert.ToInt32(requestUpdate.Phone);
                empToUpdate.NationalId = requestUpdate.NID;
                employeeService.UpdateEmployeeByAdmin(empToUpdate);
                requestUpdateService.Delete(id);
                Session["Count"] = requestUpdateService.getRowCount();
                return RedirectToAction("Inbox");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Decline(string id)
        {
            if (Session["uname"] != null)
            {
                requestUpdateService.Delete(id);
                return RedirectToAction("Inbox");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Inbox()
        {
            if (Session["uname"] != null)
            {
                return View(requestUpdateService.GetAll());
            }
            return RedirectToAction("Index", "Home");
        }
    }
}