using CSSEntity;
using System.Linq;

namespace CSSRepository
{
    public class RequestUpdateRepository : Repository<RequestUpdate>, IRequestUpdateRepository
    {
        DataContext context = DataContext.getInstance();
        public int getRowCount()
        {
            return this.context.RequestUpdates.Count();
        }
    }
}
