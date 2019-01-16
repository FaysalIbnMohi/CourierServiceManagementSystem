using CSSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CSSEntity;
using System.Net.Mail;

namespace courier_service_system.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        IEmployeeService service;
        public HomeController()
        {
            this.service = Injector.Container.Resolve<IEmployeeService>();
        }
        public ActionResult Index()
        {
            //DataContext context = DataContext.getInstance();
            if (Session["id"] == null)
                return View();
            else if (Session["Position"].ToString() == "Admin")
                return RedirectToAction("Dashboard", "Employee");
            else if (Session["Position"].ToString() == "Manager")
                return RedirectToAction("Dashboard", "Manager");
            else
                return RedirectToAction("Dashboard", "Driver");
        }

        EmployeeService employeeService = new EmployeeService();
        LoginReportService loginReportService = new LoginReportService();
        RequestUpdateService requestUpdateService = new RequestUpdateService();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public ActionResult ConfirmedLogin()
        {
            string id = Request["username"];
            string pass = Request["password"];
            Employee employee = employeeService.Get(id);
            if (employee != null)
            {
                if (pass == employee.Password && employee.CurrentStatus == "Active")
                {
                    Session["message"] = null;
                    LoginReport loginReport = new LoginReport();
                    loginReport.EmployeeId = id;
                    Session["logTime"] = loginReport.LoginTime = DateTime.Now;
                    Session["uname"] = employee.Name;
                    Session["id"] = id;
                    Session["From"] = employee.Email;
                    Session["Position"] = employee.Position;
                    loginReportService.Insert(loginReport);
                    if (id[0] == 'A')
                    {
                        Session["Count"] = requestUpdateService.getRowCount();
                        return RedirectToAction("Dashboard", "Employee");
                    }
                    else if (id[0] == 'D')
                        return RedirectToAction("Dashboard", "Driver");
                    else if (id[0] == 'M')
                        return RedirectToAction("Dashboard", "Manager");
                }
            }
            Session["message"] = "Invalid User or Password";
            return RedirectToAction("Login", "Home"); ;
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost, ActionName("ForgetPassword")]
        public ActionResult SendPassword()
        {
            Employee employee = service.Get(Request["username"]);
            if (employee != null)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(employee.Email);
                mail.From = new MailAddress("auibdebug@gmail.com");
                mail.Subject = "Recover Password";
                string Body = "Dear <b>" + employee.Name + ", </b><br/>" +
                              "Your Information:<br/>" +
                              "<label>Use Id : </label>" + employee.Id +
                              "<br/><label>Password : </label>" + employee.Password +
                              "<br/><label>Office Id : </label>" + employee.OfficeId +
                              "<br><a href=\"http://localhost:5200/Home/Login\">Click Here to Login</a>"+
                              "<br/><br/><b>With Regards</b><br/> Mr. XYZ";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("auibdebug@gmail.com", "debug1234"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);
                Session["Fmsg"] = "Your Information has been sent to your email... :)";
            }
            else
                Session["Fmsg"] = "Id not Found!!!";
            return RedirectToAction("ForgetPassword", "Home");
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
    }
}
