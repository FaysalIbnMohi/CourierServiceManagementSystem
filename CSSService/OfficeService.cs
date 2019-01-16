using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
    public class OfficeService : Service<Office>, IOfficeService
    {
        OfficeRepository repo = new OfficeRepository();
        public List<Office> GetAllForOffice(string division)
        {
            return repo.GetAllForOffice(division);
        }

        public int getRowCount()
        {
            return repo.getRowCount();
        }
    }
}
