using System;
using System.Collections.Generic;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public interface IProductService
    {
        // Tim cac san pham hien o trang home
        public List<OverViewProductHomeFlag> findAllHomeFlagProducts();

        // Tim san pham theo id
        public OverViewProductHomeFlag findProductById(string id);

        // Tim san pham theo id
        public Product findById(string id);

        // Tim cac size hien co cua san pham
        public List<int> findAllSizeOfProducts(string id);

        // Tim gia cua san pham qua size va masp
        public int findPriceBySizeAndId(int size, string id);

        // Tim ve product_detail dua tren product_id va size
        public ProductDetail findProductDetailByProductIdAndSize(int size, string product_id);

        // Tim ve chi tiet san pham theo product_id
        public ProductDetailDisplay findProductDetailDisplayByProductId(string id);

        // Tim cac san pham cung dong
        public List<OverViewProduct> findProductWithSameCategoryById(string id);

        // Tim cac review ve san pham theo product_id
        public List<CustomerReview> findAllReviewById(string id);
    }
}
