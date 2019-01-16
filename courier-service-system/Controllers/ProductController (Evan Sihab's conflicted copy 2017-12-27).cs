using CSSEntity;
using CSSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace courier_service_system.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        TripService tripService = new TripService();
        EmployeeService employeeService = new EmployeeService();
        ProductTypeService productTypeService = new ProductTypeService();
        ProductService productService = new ProductService();
        CustomerService customerService = new CustomerService();
        public ActionResult Products()
        {
            return View(tripService.GetAllForProduct(employeeService.Get(Session["id"].ToString()).OfficeId));
        }
        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(this.productService.GetAllProducts(id));
        }
        [HttpGet]
        public ActionResult AddNewType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewType(ProductType productType)
        {
            productType.Id = productType.Name[0] + productType.Name[1] + productType.Name[2] + "-" + productType.ShipmentCost;
            int i = this.productTypeService.Insert(productType);
            if(i!=0)
            {
                @TempData["TypeText"] = "Successfully Added";
            }
            return RedirectToAction("AllTypes","Product");
        }
        [HttpGet]
        public ActionResult AllTypes()
        {
            return View(this.productTypeService.GetAll());
        }
        [HttpGet]
        public ActionResult ShowDetails(string productId)
        {
            return View(this.productService.Get(productId));
        }
        [HttpGet]
        public ActionResult ReceivedDetails(string productId)
        {
            return View(this.productService.Get(productId));
        }
        [HttpGet]
        public ActionResult DeliveredDetails(string productId)
        {
            return View(this.productService.Get(productId));
        }
        [HttpGet]
        public ActionResult ReceivedProduct()
        {
            return View(this.productService.GetAllReceivedProducts());
        }
        [HttpGet]
        public ActionResult DeliveredProduct()
        {
            return View(this.productService.GetAllDeliveredProducts());
        }
        [HttpGet]
        public ActionResult TakeOder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TakeOder(Customer customer)
        {
            if(customerService.Get(customer.CustomerPhoneNumber.ToString())!=null)
            {
                customer.
                return View();
            }
            customer.CustomerId = customer
        }
    }
}