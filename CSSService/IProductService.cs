using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
    public interface IProductService : IService<Product>
    {
        int UpdateProduct(Product emp);
        List<Product> GetAllProducts(string tripId);
        List<Product> GetAllDeliveredProducts();
        List<Product> GetAllReceivedProducts();
        int UpdateStatus(string deliveryStatus, string productId);
    }
}
