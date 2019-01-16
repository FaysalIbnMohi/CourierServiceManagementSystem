using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private DataContext context;

        public CustomerRepository() { this.context = DataContext.getInstance(); }

        public int UpdateCustomer(Customer ctm)
        {
            Customer cToUpdate = this.context.Customers.SingleOrDefault(c => c.CustomerId == ctm.CustomerId);
            cToUpdate.CustomerName = ctm.CustomerName;
            cToUpdate.CustomerPhoneNumber = ctm.CustomerPhoneNumber;
            cToUpdate.CustomerEmail = ctm.CustomerEmail;
            cToUpdate.CustomerAddress = ctm.CustomerAddress;
            cToUpdate.ReceiverName = ctm.ReceiverName;
            cToUpdate.ReceiverPhoneNumber = ctm.ReceiverPhoneNumber;
            cToUpdate.ReceiverEmail = ctm.ReceiverEmail;
            cToUpdate.ReceiverAddress = ctm.ReceiverAddress;
            return this.context.SaveChanges();
        }
        public List<Customer> GetCustomer(string id)
        {
            return this.context.Customers.Where(oh => oh.CustomerId.Contains(id)).ToList();
        }
        public int UpdateProductId(string CustomerId, string productId)
        {
            Customer customer = Get(CustomerId);
            customer.ProductId = productId;
            context.Entry(customer).Property("ProductId").IsModified = true;
            return context.SaveChanges();
        }
        public int GetRowCount()
        {
            return this.context.Customers.Count();
        }
    }
}
