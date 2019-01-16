using CSSEntity;

namespace CSSService
{
    public interface IRequestUpdateService : IService<RequestUpdate>
    {
        int getRowCount();
    }
}
