using ElectronicCommerce.Areas.Admin.ViewModels;
using System.Collections.Generic;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public interface IProductService
    {
        public List<VMProductDetail> findAllProduct();
      

    }
}
