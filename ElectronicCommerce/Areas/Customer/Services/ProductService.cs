using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public class ProductService:IProductService
    {
        private DatabaseContext _db;

        public ProductService(DatabaseContext db)
        {
            _db = db;
        }

        public List<OverViewProductHomeFlag> findAllHomeFlagProducts()
        {
            var result = _db.OverViewProductHomeFlags.FromSqlRaw("[dbo].[sp_findall_home_flag_product_with_min_price]").ToList();
            return result;
        }

        public List<int> findAllSizeOfProducts(string id)
        {
            var result =_db.ProductDetails.ToList().Where(i => i.ProductId == id).Select(i => i.Size).ToList();
            return result;
        }

        public OverViewProductHomeFlag findProductById(string id)
        {
            var result = findAllHomeFlagProducts().SingleOrDefault(p => p.PRODUCT_ID == id);
            return result;
        }

        public int findPriceBySizeAndId(int size, string id)
        {
            var product_price_id = _db.ProductDetails.ToList().SingleOrDefault(i => i.Size == size && i.ProductId.Equals(id));
            int result = _db.ProductPrices.ToList().SingleOrDefault(i => i.ProductPriceId.Equals(product_price_id.ProductPriceId)).SalePrice;
            return result;
        }
    }
}
