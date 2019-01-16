using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository 
{
    public class TripRepository : Repository<Trip> , ITripRepository
    {
        private DataContext context;

        public TripRepository() { this.context = DataContext.getInstance(); }


        public List<Trip> GetAllById(string id)
        {
            return this.context.Trips.Where(e => e.Id == id).ToList();
        }

        public int UpdateTrip(Trip tp)
        {
            Trip tpToUpdate = this.context.Trips.SingleOrDefault(t => t.TripId == t.TripId);
            tpToUpdate.StartOfficeId = tp.StartOfficeId;
            tpToUpdate.DestinationOfficeId= tp.DestinationOfficeId;
            tpToUpdate.Id = tp.Id;
            return this.context.SaveChanges();
        }

        public string GetOfficeLocation(string offcieId)
        {
            string Devision = offcieId[0].ToString() + offcieId[1].ToString() + offcieId[2].ToString();
            if(Devision.Equals("Dha"))
            {
                Devision = "Dhaka,Bangladesh";
            }
            if (Devision.Equals("Chi"))
            {
                Devision = "Chittagong,Bangladesh";
            }
            if (Devision.Equals("Bar"))
            {
                Devision = "Barisal,Bangladesh";
            }
            if (Devision.Equals("Mym"))
            {
                Devision = "Mymensingh,Bangladesh";
            }
            if (Devision.Equals("Khu"))
            {
                Devision = "Khulna,Bangladesh";
            }
            if (Devision.Equals("Raj"))
            {
                Devision = "Rajshahi,Bangladesh";
            }
            if (Devision.Equals("Ran"))
            {
                Devision = "Rangpur,Bangladesh";
            }
            if (Devision.Equals("Syl"))
            {
                Devision = "Sylhet,Bangladesh";
            }
            return Devision;
        }
        public List<Trip> GetAllForProduct(string ofcId)
        {
            return this.context.Trips.Where(e => e.DestinationOfficeId == ofcId).ToList();
        }
        public Trip GetTripIdForProduct(DateTime date ,String destinationId,String startId)
        {
            return this.context.Trips.SingleOrDefault(e => System.Data.Entity.DbFunctions.TruncateTime(e.TripDate) == date && e.DestinationOfficeId == destinationId && e.StartOfficeId == startId);
        }
    }
}
