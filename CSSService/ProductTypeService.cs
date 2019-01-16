using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSService
{
    public class ProductTypeService : Service<ProductType>,IProductTypeService
    {
        private DataContext context;

        public ProductTypeService() { this.context = DataContext.getInstance(); }

        public int UpdateProduct(ProductType pt)
        {
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            return productTypeRepository.UpdateProduct(pt);
        }
    }
}
