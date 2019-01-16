using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        int UpdateCustomer(Customer ctm);
        List<Customer> GetCustomer(string id);
        int UpdateProductId(string CustomerId, string productId);
    }
}
