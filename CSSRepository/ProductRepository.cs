using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
   public class ProductRepository : Repository<Product>, IProductRepository
    {
        private DataContext context;

        public ProductRepository() { this.context = DataContext.getInstance(); }


        public int UpdateProduct(Product p)
        {
            Product pToUpdate = this.context.Products.SingleOrDefault(pd => pd.ProductId == p.ProductId);
            pToUpdate.OfficeIdTo = p.OfficeIdTo;
            return this.context.SaveChanges();
        }
        public List<Product> GetAllProducts(string tripId)
        {
            return this.context.Products.Where(pd => pd.TripId == tripId).ToList();
        }
        public List<Product> GetAllReceivedProducts()
        {
            return this.context.Products.Where(pd => pd.DeliveryStatus == "Received").ToList();
        }
        public List<Product> GetAllDeliveredProducts()
        {
            return (this.context.Products.Where(pd => pd.DeliveryStatus == "Delivered")).ToList();
        }
        public int UpdateStatus(string deliveryStatus,string productId)
        {
            Product product = Get(productId);
            product.DeliveryStatus = deliveryStatus;
            context.Entry(product).Property("DeliveryStatus").IsModified = true;
            return context.SaveChanges();
        }
        public int GetRowCount()
        {
            return this.context.Products.Count();
        }
    }
}
