using System;
using System.Collections.Generic;
using ElectronicCommerce.Areas.Customer.Models;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public interface IProductService
    {
        // Tim cac san pham hien o trang home
        public List<OverViewProductHomeFlag> findAllHomeFlagProducts();

        // Tim san pham theo id
        public OverViewProductHomeFlag findProductById(string id);

        // Tim cac size hien co cua san pham
        public List<int> findAllSizeOfProducts(string id);

        // Tim gia cua san pham qua size va masp
        public int findPriceBySizeAndId(int size, string id);

    }
}
