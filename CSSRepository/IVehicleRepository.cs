using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        int UpdateVehicle(Vehicle vcl, string id);
        List<Vehicle> GetAllByLocation(string StartLocation);
    }

}
