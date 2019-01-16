using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService 
{
    public class TripService : Service<Trip> , ITripService
    {
        TripRepository repo = new TripRepository();

        public List<Trip> GetAllById(string id)
        {
            return repo.GetAllById(id);
        }

        public int UpdateTrip(Trip tp)
        {
            return repo.UpdateTrip(tp);
        }

        public string GetOfficeLocation(string offcieId)
        {
            return repo.GetOfficeLocation(offcieId);
        }
        public List<Trip> GetAllForProduct(string ofcId)
        {
            return repo.GetAllForProduct(ofcId);
        }
        public Trip GetTripIdForProduct(DateTime date, String destinationId, String startId)
        {
            return repo.GetTripIdForProduct(date, destinationId, startId);
        }
    }
}
