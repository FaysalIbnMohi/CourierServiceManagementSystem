using CSSEntity;
using CSSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace courier_service_system.Controllers
{

    public class TripController : Controller
    {

        OfficeService officeService = new OfficeService();
        VehicleService vehicleService = new VehicleService();
        EmployeeService employeeService = new EmployeeService();
        // GET: Trip
        TripService tripService = new TripService();
        [HttpGet]
        public ActionResult Trips()
        {
            return View(this.tripService.GetAll());
        }
        public ActionResult OngoingTrips()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AssignNewTrip()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AssignNewTrip(Trip trip)
        {
            Session["FinalTripDate"] = Request["tripDate"].ToString();
            return RedirectToAction("AssignTripDestination","Trip");
        }
        [HttpGet]
        public ActionResult AssignTripDestination()
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
        public ActionResult AssignTripDestination(Trip trip)
        {
            if (trip.StartOfficeId == trip.DestinationOfficeId)
            {
                TempData["Warning"] = "Start Point and Destionation Point Cannot be Same";
                return RedirectToAction("AssignTripDestination", "Trip");
            }
            else
            {
                Session["TripStart"] = trip.StartOfficeId;
                Session["TripDestination"] = trip.DestinationOfficeId;
                return RedirectToAction("AssignTripEmployeeVehicle", "Trip");
            }
        }
        [HttpGet]
        public ActionResult AssignTripEmployeeVehicle()
        {
            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            foreach (Employee employee in employeeService.GetAllDrivers(Session["TripStart"].ToString()))
            {
                SelectListItem option = new SelectListItem();
                option.Text = employee.Id;
                option.Value = employee.Id;
                EmployeeList.Add(option);
            }

            List<SelectListItem> VehicleList = new List<SelectListItem>();
            foreach (Vehicle vehicle in vehicleService.GetAllByLocation(Session["TripStart"].ToString()))
            {
                SelectListItem option = new SelectListItem();
                option.Text = vehicle.VehicleNumber;
                option.Value = vehicle.VehicleNumber;
                VehicleList.Add(option);
            }
            ViewBag.EmployeeList = EmployeeList;
            ViewBag.VehicleList = VehicleList;
            TempData["TripDate"] = Session["FinalTripDate"];
            TempData["StartOfficeId"] = Session["TripStart"].ToString();
            TempData["DestinationOfficeId"] = Session["TripDestination"].ToString();
            return View();
        }
        [HttpPost]
        public ActionResult AssignTripEmployeeVehicle(Trip trip)
        {
            Session.Remove("FinalTripDate");
            Session.Remove("TripStart");
            Session.Remove("TripDestination");
            trip.StartOfficeId = TempData["StartOfficeId"].ToString();
            trip.DestinationOfficeId = TempData["DestinationOfficeId"].ToString();
            trip.TripId = trip.StartOfficeId[0].ToString() + trip.DestinationOfficeId[0].ToString() + "-" + trip.Id[0]+ trip.Id[trip.Id.Length - 3]+trip.Id[trip.Id.Length - 2]+trip.Id[trip.Id.Length-1] + "-" + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            trip.TripDate = Convert.ToDateTime(TempData["TripDate"]).Date;
            tripService.Insert(trip);
            return RedirectToAction("Trips", "Trip");
        }

        [HttpGet]
        public ActionResult ShowDirection(string id)
        {
            Trip trip = tripService.Get(id);
            string StartOffice = trip.StartOfficeId;
            string EndOffice = trip.DestinationOfficeId;
            TempData["StartPoint"] = tripService.GetOfficeLocation(StartOffice);
            TempData["EndPoint"] = tripService.GetOfficeLocation(EndOffice);
            return View();
        }
    }
}