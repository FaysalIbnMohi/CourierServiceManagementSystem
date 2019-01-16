//using DataLayer;
using CSSEntity;
using CSSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
namespace courier_service_system.Controllers
{
    public class SendMailerController : Controller
    {
        //  
        // GET: /SendMailer/  
        EmployeeService employeeService = new EmployeeService();
        MailModelService mailModelService = new MailModelService();
        public ActionResult Confirmation(string id)
        {
            Employee employee = employeeService.Get(id);
            MailMessage mail = new MailMessage();
            mail.To.Add(employee.Email);
            mail.From = new MailAddress("auibdebug@gmail.com");
            mail.Subject = "Registration Confirmation";
            string Body = "Dear <b>" + employee.Name + ", </b><br/>"+
                          "Welcome to Our Family. Please keep in mind below information...<br/>" + 
                          "<label>Use Id : </label>" + employee.Id + 
                          "<br/><label>Password : </label>" + employee.Password + 
                          "<br/><label>Office Id : </label>" + employee.OfficeId +
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
	        if (Session["CurrentState"].Equals("Office"))
            {
                return RedirectToAction("Result", "Office",new { id = employee.Id });
            }
            else
            {
                return RedirectToAction("Result", "Employee");
            }
        }

        public ActionResult ProductConfirmationMail(string id)
        {
            ProductService productService = new ProductService();
            CustomerService customerService = new CustomerService();
            OfficeService officeService = new OfficeService();
            Product product = productService.Get(id);
            Customer customer = customerService.Get(product.CustomerId);
            Office office = officeService.Get(product.OfficeIdFrom);
            MailMessage mail = new MailMessage();
            mail.To.Add(customer.CustomerEmail);
            mail.From = new MailAddress("auibdebug@gmail.com");
            mail.Subject = "Oder Confirmation";
            string Body = "Dear <b>" + customer.CustomerName + ", </b><br/>" +
                          "Thank you for choosing us. Please keep the following information...<br/>" +
                          "<label>Product Code #: </label>" + product.ProductId +
                          "<br/><label>Product Type : </label>" + product.ProductType +
                          "<br/><label>Quantity : </label>" + product.ProductQuantity +
                          "<br/><label>Total Weight : </label>" + product.ProductWeight + " Unit(s)" +
                          "<br/><label>Shipping Cost : </label>" + product.SendingCost + " TK" +
                          "<br/><label>Sending To : </label>" + customer.ReceiverName + 
                          "<br/><br/><b>For Any Query @Call : </b>"+office.OfficialNumber+"<br/>";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("auibdebug@gmail.com", "debug1234"); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);
            return RedirectToAction("ProductFinalConfirmation", "Product",new { id = id });
        }

        public ActionResult ReceiveConfirmationMail(string id)
        {
            ProductService productService = new ProductService();
            CustomerService customerService = new CustomerService();
            OfficeService officeService = new OfficeService();
            Product product = productService.Get(id);
            Customer customer = customerService.Get(product.CustomerId);
            Office office = officeService.Get(product.OfficeIdFrom);
            MailMessage mail = new MailMessage();
            mail.To.Add(customer.ReceiverEmail);
            mail.From = new MailAddress("auibdebug@gmail.com");
            mail.Subject = "Product Received";
            string Body = "Dear <b>" + customer.ReceiverName + ", </b><br/>" +
                          "We just received a product for you that was send by "+customer.CustomerName+".<br> Please come to our Office with the Product Code below and also bring your Phone 0"+customer.ReceiverPhoneNumber+".<br/>" +
                          "<b><label>Product Code #: </label>" + product.ProductId +"<b>"+
                          "<br/><label>Product Type : </label>" + product.ProductType +
                          "<br/><label>Quantity : </label>" + product.ProductQuantity +
                          "<br/><label> Office Location : </label>" +office.Location +
                          "<br/><br/><b>For Any Query @Call : </b>" + office.OfficialNumber + "<br/>";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("auibdebug@gmail.com", "debug1234"); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);
            @TempData["ReceivingMessage"] = " Successfully Received";
            return RedirectToAction("ReceiveProduct", "Product");
        }

        [HttpGet]
        public ActionResult MailBox(string id)
        {
            if (id == null)
                return View();
            Employee empToUpdate = employeeService.Get(Session["Id"].ToString());
            ViewBag.To = "Admin";
            ViewBag.Subject = "Update Information";
            //empToUpdate.PhoneNumber = Convert.ToInt32(Request["PhoneNumber"]);
            //empToUpdate.NationalId = Request["NationalId"];
            //empToUpdate.Birthday = Convert.ToDateTime(Request["Birthday"]);
            //empToUpdate.Address = Request["Address"];
            //empToUpdate.Name = Request["Name"];
            ViewBag.Body = "Name : " + TempData["Name"] + "\n" +
                           "Phone Number : " + TempData["PhoneNumber"] + "\n" +
                           "National Id : " + TempData["NationalId"] + "\n" +
                           "Birtday : " + TempData["Birthday"] + "\n" +
                           "Address : " + TempData["Address"] + "\n";
            return View();
        }
        [HttpPost, ActionName("MailBox")]
        public ActionResult SendMail()
        {
            MailModelService mailModel = new MailModelService();
            MailModel model = new MailModel();
            model.To = Request["to"];
            model.From = Session["From"].ToString();
            model.Subject = Request["subject"];
            model.Body = Request["body"];
            model.Date = DateTime.Now;
            mailModelService.InsertMail(model);
            ViewBag.status = "Mail Has been Sent Successfully";
            return View();
        }
        public ActionResult Inbox()
        {

            return View(this.mailModelService.GetAllById(Session["From"].ToString(), Session["Position"].ToString()));
        }
        public ActionResult Details(string id)
        {
            return View(mailModelService.Get(id));
        }
        [HttpPost]
        public  string Delete()
        {
            int id = Convert.ToInt32(Request["id"]);
            mailModelService.DeleteMail(id);
            return "Successfully Deleted";
        }
    }
}