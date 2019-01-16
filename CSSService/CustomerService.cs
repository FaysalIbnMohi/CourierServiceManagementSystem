using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
    public class CustomerService : Service<Customer>, ICustomerService
    {
        CustomerRepository repo = new CustomerRepository();

        public int UpdateCustomer(Customer ctm)
        {
            return repo.UpdateCustomer(ctm);
        }
        public List<Customer> GetCustomer(string id)
        {
            return repo.GetCustomer(id);
        }
        public int UpdateProductId(string CustomerId, string productId)
        {
            return repo.UpdateProductId(CustomerId, productId);
        }
        public int GetRowCount()
        {
            return repo.GetRowCount();
        }
    }
}
