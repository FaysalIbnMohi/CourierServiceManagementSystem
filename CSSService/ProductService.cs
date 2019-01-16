using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
   public class ProductService : Service<Product>, IProductService
    {
        ProductRepository repo = new ProductRepository();
        public int UpdateProduct(Product p)
        {
            return repo.UpdateProduct(p);
        }
        public List<Product> GetAllProducts(string tripId)
        {
            return repo.GetAllProducts(tripId);
        }
        public List<Product> GetAllReceivedProducts()
        {
            return repo.GetAllReceivedProducts();
        }
        public List<Product> GetAllDeliveredProducts()
        {
            return repo.GetAllDeliveredProducts();
        }
        public int UpdateStatus(string deliveryStatus, string productId)
        {
            return repo.UpdateStatus(deliveryStatus, productId);
        }
        public int GetRowCount()
        {
            return repo.GetRowCount();
        }
    }
}
