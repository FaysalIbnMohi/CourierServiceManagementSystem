using CSSEntity;
using CSSRepository;

namespace CSSService
{
    public class RequestUpdateService : Service<RequestUpdate>, IRequestUpdateService
    {
        RequestUpdateRepository repo = new RequestUpdateRepository();
        public int getRowCount()
        {
            return repo.getRowCount();
        }
    }
}
