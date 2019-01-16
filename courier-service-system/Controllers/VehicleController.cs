using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSSService;
using CSSEntity;

namespace courier_service_system.Controllers
{
    public class VehicleController : Controller
    {
        // GET: Vehicle
        EmployeeService employeeService = new EmployeeService();
        VehicleService vehicleService = new VehicleService();
        OfficeService officeService = new OfficeService();
        public ActionResult AllVehicle()
        {
            Session["VehicleNumber"] = null;
            Session.Remove("VehicleNumber");
            return View(this.vehicleService.GetAll());
        }
        [HttpGet]
        public ActionResult AddNewVehicle()
        {
            List<SelectListItem> OfficeList = new List<SelectListItem>();
            foreach (Office office in officeService.GetAll())
            {
                SelectListItem option = new SelectListItem();
                option.Text = office.OfficeId;
                option.Value = office.OfficeId;
                OfficeList.Add(option);
            }
            ViewBag.OfficeList = OfficeList;
            return View();
        }
        [HttpPost]
        public ActionResult AddNewVehicle(Vehicle vehicle)
        {
                int i = 0;
                vehicle.VehicleRegisteredByEmployee = Session["id"].ToString();
                vehicle.VehicleRegisterDate = DateTime.Now;
                vehicle.VehicleEntryByEmployeeId = Session["id"].ToString();
                i = vehicleService.Insert(vehicle);
                if (i != 0)
                {
                    TempData["message"] = "Successfully Added";
                }
                return RedirectToAction("AllVehicle", "Vehicle");
            
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Vehicle vcl = this.vehicleService.Get(id);
            Session["VehicleNumber"] = vcl.VehicleNumber;
            return View(vcl);
        }
        [HttpPost]
        public ActionResult Edit(Vehicle vehicle)
        {
            int i = 0;
            i = this.vehicleService.UpdateVehicle(vehicle, Session["VehicleNumber"].ToString());
            if(i!=0)
            {
                TempData["message"] = "Successfully Updated";
            }
            return RedirectToAction("AllVehicle","Vehicle");
        }
        [HttpGet]
        public ActionResult Details(string id)
        {
            return View(this.vehicleService.Get(id));
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            return View(this.vehicleService.Get(id));
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            int i = 0;
            i = vehicleService.Delete(id);
            if (i != 0)
            { TempData["message"] = "Successfully Deleted"; }
            return RedirectToAction("AllVehicle", "Vehicle");
        }
    }
}