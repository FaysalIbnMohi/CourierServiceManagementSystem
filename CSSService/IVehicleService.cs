using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
    public interface IVehicleService : IService<Vehicle>
    {
        int UpdateVehicle(Vehicle vcl, string id);
    }
}
