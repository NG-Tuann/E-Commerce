using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public class ProductService : IProductService
    {
        private DatabaseContext _db;
        public ProductService(DatabaseContext db)
        {
            _db = db;
        }
        public List<VMProductDetail> findAllProduct()
        {
            return _db.VMProductDetails.FromSqlRaw("[dbo].[findAllProduct]").ToList();

        }
    }
}
