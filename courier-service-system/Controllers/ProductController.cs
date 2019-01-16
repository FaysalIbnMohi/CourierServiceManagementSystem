using CSSEntity;
using CSSService;
using Rotativa;
using System;
using System.Collections.Generic;
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
            productType.Id = productType.Name[0].ToString() + productType.Name[1].ToString() + productType.Name[2].ToString() + "-" + productType.ShipmentCost;
            int i = this.productTypeService.Insert(productType);
            if (i != 0)
            {
                @TempData["TypeText"] = "Successfully Added";
            }
            return RedirectToAction("AllTypes", "Product");
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
        public ActionResult ReceivedDetails(string id)
        {
            return View(customerService.Get(id));
        }
        [HttpGet]
        public ActionResult DeliveredDetails(string id)
        {
            return View(customerService.Get(id));
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
            List<Customer> customerlist = customerService.GetCustomer(customer.CustomerPhoneNumber.ToString());
            if (customerlist.Count != 0)
            {
                TempData["CustomerName"] = customerlist[0].CustomerName;
                TempData["CustomerPhoneNumber"] = customerlist[0].CustomerPhoneNumber;
                TempData["CustomerEmail"] = customerlist[0].CustomerEmail;
                TempData["CustomerAddress"] = customerlist[0].CustomerAddress;
                return RedirectToAction("CreateReceiver", "Product");
            }
            else
            {
                TempData["CustomerPhoneNumber"] = customer.CustomerPhoneNumber;
                return RedirectToAction("CreateCustomerReSceiver", "Product");
            }
        }
        [HttpGet]
        public ActionResult CreateCustomerReSceiver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomerReSceiver(Customer customer)
        {
            customer.ReceiverAddress = Request["country"] + "," + Request["city"];
            customer.CustomerId = customer.CustomerPhoneNumber + "-" + DateTime.Now.Date.ToShortDateString() + "-" + customerService.GetRowCount();
            customer.ProductId = "none";
            customerService.Insert(customer);
            Trip tripCheck = tripService.GetTripIdForProduct(DateTime.Now.Date, Request["city"].ToString(), employeeService.Get(Session["id"].ToString()).OfficeId);
            if (tripCheck == null)
            {
                tripCheck = tripService.GetTripIdForProduct(DateTime.Now.Date.AddDays(1), Request["city"].ToString(), employeeService.Get(Session["id"].ToString()).OfficeId);
                if (tripCheck == null)
                {
                    Session["ProductTripId"] = "No Available Trip";
                }
                else
                {
                    Session["ProductTripId"] = tripCheck.TripId;
                }
            }
            else
            {
                Session["ProductTripId"] = tripCheck.TripId;
            }
            Session["ReceiverOffice"] = Request["city"];
            Session["CustomerId"] = customer.CustomerId;
            return RedirectToAction("TakeProduct", "Product");
        }

        [HttpGet]
        public ActionResult TakeProduct()
        {
            List<SelectListItem> ProductTypeList = new List<SelectListItem>();
            foreach (ProductType producttype in productTypeService.GetAll())
            {
                SelectListItem option = new SelectListItem();
                option.Text = producttype.Name;
                option.Value = producttype.Id;
                ProductTypeList.Add(option);
            }
            ViewBag.ProductTypeList = ProductTypeList;
            return View();
        }

        [HttpPost]
        public ActionResult TakeProduct(Product product)
        {
            if (product.ProductWeight > 0 && product.ProductQuantity > 0)
            {
                ProductType productType = productTypeService.Get(product.ProductType);
                Session["ProductType"] = productTypeService.Get(product.ProductType).Name;
                Session["ProductQuantity"] = product.ProductQuantity;
                Session["ProductWeight"] = product.ProductWeight;
                double costCalculation = productType.ShipmentCost * (Convert.ToInt16(product.ProductQuantity) * Convert.ToInt16(product.ProductWeight));
                Session["ShipmentCost"] = Math.Round(costCalculation + (costCalculation*(Convert.ToDouble(productType.Vat)/100.0)));
                return RedirectToAction("TakeShipmentCost", "Product");
            }
            else
            {
                TempData["InvalidWeight"] = "Invalid Weight or Quantity";
                return RedirectToAction("TakeProduct", "Product");
            }
        }
        [HttpGet]
        public ActionResult TakeShipmentCost()
        {
            Session["ProductId"] = Session["ProductType"].ToString()[0] + Session["ProductType"].ToString()[1] + Session["ProductType"].ToString()[2] + Session["ProductQuantity"].ToString() + productService.GetRowCount().ToString();
            return View();
        }

        [HttpPost]
        public ActionResult TakeShipmentCost(Product product)
        {
            if(product.GivenMoney < Convert.ToInt16(Session["ShipmentCost"]))
            {
                TempData["InvalidMoney"] = "Given Money is Low , Need " + (Convert.ToInt16(Session["ShipmentCost"]) - product.GivenMoney) + " Tk More";
                return RedirectToAction("TakeShipmentCost", "Product");
            }
            else
            {
                TempData["ReturnMoney"] = -Convert.ToInt16(Session["ShipmentCost"]) + product.GivenMoney;
                Product finalProduct = new Product();
                finalProduct.ProductId = Session["ProductId"].ToString();
                finalProduct.OfficeIdFrom = employeeService.Get(Session["id"].ToString()).OfficeId;
                finalProduct.OfficeIdTo = Session["ReceiverOffice"].ToString();
                finalProduct.TripId = Session["ProductTripId"].ToString();
                finalProduct.CustomerId = Session["CustomerId"].ToString();
                finalProduct.ProductType = Session["ProductType"].ToString();
                finalProduct.ProductQuantity = Convert.ToInt16(Session["ProductQuantity"]);
                finalProduct.ProductWeight = Convert.ToInt16(Session["ProductWeight"]);
                finalProduct.SendingCost = Convert.ToInt16(Session["ShipmentCost"]);
                finalProduct.GivenMoney = product.GivenMoney;
                finalProduct.ReturnMoney = Convert.ToInt16(TempData["ReturnMoney"]);
                finalProduct.OderDate = DateTime.Now;
                finalProduct.OderTakenEmployeeId = Session["id"].ToString();
                finalProduct.DeliveryStatus = "OnWayProduct";
                int i = productService.Insert(finalProduct);
                if(i!=0)
                {
                    customerService.UpdateProductId(Session["CustomerId"].ToString(), Session["ProductId"].ToString());
                }
                return RedirectToAction("ProductConfirmationMail", "SendMailer", new { id = Session["ProductId"] });
                
            }
        }
        [HttpGet]
        public ActionResult ProductFinalConfirmation(string id) 
        {
            Product pro = productService.Get(id);
            return View(pro);
        }
        public ActionResult PrintRecipt(string id)
        {
            var report = new ActionAsPdf("PaymentSlip", new { id = id});
            return report;
        }
        public ActionResult PaymentSlip(string id)
        {
            Trip trip = tripService.Get(productService.Get(id).TripId);
            TempData["VehicleNumber"] = trip.VehicleNumber;
            Product pro = productService.Get(id);
            Session.Remove("ShipmentCost");
            Session.Remove("ProductWeight");
            Session.Remove("ProductQuantity");
            Session.Remove("ProductType");
            Session.Remove("CustomerId");
            Session.Remove("ProductTripId");
            Session.Remove("ReceiverOffice");
            Session.Remove("ProductId");
            return View(pro);
        }
        [HttpGet]
        public ActionResult CreateReceiver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateReceiver(Customer customer)
        {
            customer.ReceiverAddress = Request["country"] + "," + Request["city"];
            customer.CustomerId = customer.CustomerPhoneNumber + "-" + DateTime.Now.Date.ToShortDateString() + "-" + customerService.GetRowCount();
            customer.ProductId = "none";
            customerService.Insert(customer);
            
            Trip tripCheck = tripService.GetTripIdForProduct(DateTime.Now.Date, Request["city"].ToString(), employeeService.Get(Session["id"].ToString()).OfficeId);
            if (tripCheck == null)
            {
                tripCheck = tripService.GetTripIdForProduct(DateTime.Now.Date.AddDays(1), Request["city"].ToString(), employeeService.Get(Session["id"].ToString()).OfficeId);
                if (tripCheck == null)
                {
                    tripCheck = tripService.GetTripIdForProduct(DateTime.Now.Date.AddDays(2), Request["city"].ToString(), employeeService.Get(Session["id"].ToString()).OfficeId);
                    if(tripCheck == null)
                    {
                        Session["ProductTripId"] = "No Available Trip";
                    }
                    else
                    {
                        Session["ProductTripId"] = tripCheck.TripId;
                    }
                }
                else
                {
                    Session["ProductTripId"] = tripCheck.TripId;
                }
            }
            else
            {
                Session["ProductTripId"] = tripCheck.TripId;
            }
            Session["ReceiverOffice"] = Request["city"];
            Session["CustomerId"] = customer.CustomerId;
            return RedirectToAction("TakeProduct", "Product");
        }

        [HttpGet]
        public ActionResult OderDelivery()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OderDelivery(Product product)
        {
                Product ReceiveProduct = productService.Get(product.ProductId);
                if (ReceiveProduct.DeliveryStatus == "Deliverd")
                {
                    TempData["ProductDeliveryMessage"] = "Product Already Deliverd";
                    return RedirectToAction("OderDelivery", "Product");
                }
                else
                {
                    Customer customerInfo = customerService.Get(ReceiveProduct.CustomerId);
                    if ((Convert.ToInt32(Request["PhoneNumber"]) == customerInfo.ReceiverPhoneNumber) && (ReceiveProduct != null) && (ReceiveProduct.OfficeIdTo == employeeService.Get(Session["id"].ToString()).OfficeId))
                    {
                        Session["customerReceiverInfo"] = customerInfo.ReceiverName;
                        return RedirectToAction("ConfirmDelivery", "Product", new { id = ReceiveProduct.ProductId });
                    }
                    else
                    {
                        TempData["ProductDeliveryMessage"] = "Wrong Product-Id or Phone Number";
                        return RedirectToAction("OderDelivery", "Product");
                    }
                }
        }
        [HttpGet]
        public ActionResult ConfirmDelivery(string id)
        {
            Product product = productService.Get(id);
            return View(product);
        }
        public ActionResult FinalConfirmDelivery(string id)
        {
            Product product = productService.Get(id);
            if (product.DeliveryStatus.Contains("Received"))
            {
                int i = productService.UpdateStatus("Delivered", id);
                if (i != 0)
                {
                    TempData["DeliveryStatusMessage"] = "Successfully Delivered";
                    Session.Remove("customerReceiverInfo");
                }
                return RedirectToAction("OderDelivery", "Product");
            }
            else
            {
                TempData["DeliveryStatusMessage"] = "You Cannot Deliver any On Way Products";
                return RedirectToAction("ConfirmDelivery", "Product",new { id = id });
            }
        }
        [HttpPost,ActionName("ConfirmDelivery")]
        public ActionResult FinalConfirmDelivery(Customer customer)
        {
            int i = productService.UpdateStatus("Deliverd",customer.ProductId);
            if (i != 0)
            {
                TempData["ProductDeliveryMessage"] = "Delivery Done";
            }
            return RedirectToAction("OderDelivery", "Product");
        }
        [HttpGet]
        public ActionResult ReceiveProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReceiveProduct(Product product)
        {
            Product pro = productService.Get(product.ProductId);
            if (pro == null)
            {
                @TempData["ReceivingMessage"] = "Cannot Find Any Product With this Id";
                return RedirectToAction("ReceiveProduct", "Product");
            }
            else
            {
                return RedirectToAction("FinalReceiveProduct", "Product",new { id = pro.ProductId });
            }
        }
        [HttpGet]
        public ActionResult FinalReceiveProduct(string id)
        {
            Product pro = productService.Get(id);
            return View(pro);
        }
        public ActionResult ConfirmReceiveProduct(string id)
        {
            Product pro = productService.Get(id);
            if (pro.DeliveryStatus == "OnWayProduct")
            {
                productService.UpdateStatus("Received", id);
                Customer customerReceiver = customerService.Get(pro.CustomerId);
                Trip trip = tripService.Get(pro.TripId);
                Employee employee = employeeService.Get(trip.Id);
                if (employee.CurrentLocation != pro.OfficeIdTo)
                {
                    employeeService.UpdateDriverCurrentLocation(pro.OfficeIdTo, trip.Id);
                }
                //code here
                return RedirectToAction("ReceiveConfirmationMail", "SendMailer", new { id = pro.ProductId });

            }
            if( pro.DeliveryStatus == "Received")
            {
                @TempData["ReceivingMessage"] = " This Product Is Already Received";
                return RedirectToAction("ReceiveProduct", "Product");
            }
            else
            {
                @TempData["ReceivingMessage"] = " Product is already Delivered to the Receiver";
                return RedirectToAction("ReceiveProduct", "Product");
            }
        }
    }
}