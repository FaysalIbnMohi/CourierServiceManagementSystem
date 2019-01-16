using System;
using CSSEntity;

namespace CSSRepository
{
    public interface IRequestUpdateRepository : IRepository<RequestUpdate>
    {
        int getRowCount();
    }
}
