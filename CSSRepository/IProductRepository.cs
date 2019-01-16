using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
    interface IProductRepository : IRepository<Product>
    {
        int UpdateProduct(Product emp);
        List<Product> GetAllProducts(string tripId);
        List<Product> GetAllReceivedProducts();
        List<Product> GetAllDeliveredProducts();
        int UpdateStatus(string deliveryStatus, string productId);
    }
}
