using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
    public class OfficeRepository : Repository<Office>, IOfficeRepository
    {
        private DataContext context;

        public OfficeRepository() { this.context = DataContext.getInstance(); }

        public List<Office> GetAllForOffice(string division)
        {
            return this.context.Offices.Where(oh => oh.Location.Contains(division)).ToList();
        }

        public int getRowCount()
        {
            return this.context.Employees.Count();
        }
    }
}
