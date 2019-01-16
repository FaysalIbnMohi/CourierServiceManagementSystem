using CSSEntity;
using CSSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace courier_service_system.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        EmployeeService employeeService = new EmployeeService();
        TripService tripService = new TripService();
        MailModelService mailModelService = new MailModelService();
        RequestUpdateService requestUpdateService = new RequestUpdateService();
        public ActionResult Dashboard()
        {
            if (Session["uname"] != null)
            {
                Employee employee = employeeService.Get(Session["Id"].ToString());
                ViewBag.SinceUser = Math.Round((DateTime.Now - employee.RegistrationTime).TotalDays, 2);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Trips()
        {
            return View(tripService.GetAllById(Session["Id"].ToString()));
        }
        public ActionResult ViewProfile()
        {
            Employee employee = employeeService.Get(Session["Id"].ToString());
            return View(employee);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            Employee employee = employeeService.Get(Session["Id"].ToString());
            return View(employee);
        }
        [HttpPost, ActionName("EditProfile")]
        public ActionResult SendRequeset()
        {
            MailModel mailModel = new MailModel();
            mailModel.To = "Admin";
            mailModel.From = Session["id"].ToString();
            mailModel.Subject = "Update Request";
            mailModel.Body = "<a>You Have got an Update Request From " + mailModel.From + "</a>";
            mailModel.Date = DateTime.Now;
            mailModelService.InsertMail(mailModel);
            RequestUpdate requestUpdate = new RequestUpdate();
            requestUpdate.Phone = Request["PhoneNumber"];
            requestUpdate.NID = Request["NationalId"];
            requestUpdate.DOB = Convert.ToDateTime(Request["Birthday"]);
            requestUpdate.Address = Request["Address"];
            requestUpdate.Name = Request["Name"];
            requestUpdate.Id = Session["id"].ToString();
            if (requestUpdateService.Get(requestUpdate.Id) != null)
            {
                requestUpdateService.Delete(requestUpdate.Id);
                requestUpdateService.Insert(requestUpdate);
            }
            else
            {
                requestUpdateService.Insert(requestUpdate);
            }
            TempData["Reply"] = "Your request has been sent to the proper authority";
            return RedirectToAction("EditProfile", "Driver");
        }
    }
}