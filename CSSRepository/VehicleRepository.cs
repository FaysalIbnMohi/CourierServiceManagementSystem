using CSSEntity;
using System.Collections.Generic;
using System.Linq;

namespace CSSRepository
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        private DataContext context;

        public VehicleRepository() { this.context = DataContext.getInstance(); }

        public int UpdateVehicle(Vehicle vcl, string id)
        {
           Delete(id);
           int i = Insert(vcl);
           return i;
        }
        public List<Vehicle> GetAllByLocation(string StartLocation)
        {
            return this.context.Vehicles.Where(oh => oh.CurrentLocation.Contains(StartLocation)).ToList(); ;
        }
    }
}
