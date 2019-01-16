using CSSEntity;
using CSSRepository;
using System.Collections.Generic;
using System.Linq;

namespace CSSService
{
    public class VehicleService : Service<Vehicle>, IVehicleService
    {
        VehicleRepository repo = new VehicleRepository();
        public int UpdateVehicle(Vehicle vcl, string id)
        {
            return repo.UpdateVehicle(vcl,id);
        }
        public List<Vehicle> GetAllByLocation(string StartLocation)
        {
            return repo.GetAllByLocation(StartLocation);
        }

    }
}
