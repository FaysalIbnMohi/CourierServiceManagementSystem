using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
    interface ITripRepository: IRepository<Trip>
    {
        int UpdateTrip(Trip tp);
        List<Trip> GetAllForProduct(string ofcId);
        Trip GetTripIdForProduct(DateTime date, String destinationId, String startId);
    }
}
