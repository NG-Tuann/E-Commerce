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

        public ProductDetail findProductDetailByProductIdAndSize(int size, string product_id)
        {
            ProductDetail product_detail;
            if(size ==0)
            {
                product_detail = _db.ProductDetails.ToList().SingleOrDefault(i => i.ProductId.Equals(product_id));
            }
            else
            {
                product_detail = _db.ProductDetails.ToList().SingleOrDefault(i => i.Size == size && i.ProductId.Equals(product_id));
            }

            // Lay ve nhung truong du lieu can lay
            ProductDetail result = new ProductDetail();
            // Chi ra result co doi tuong product va product price
            // Neu ko se bi loi null object reference khi gan id product va sale price cho result

            result.Product = new Product();
            result.ProductPrice = new ProductPrice();

            result.ProductDetailId = product_detail.ProductDetailId;
            result.Product.Id = product_detail.Product.Id;
            result.Size = product_detail.Size;
            result.Quantity = product_detail.Quantity;
            result.ProductPrice.SalePrice = product_detail.ProductPrice.SalePrice;
            result.Product.Image = product_detail.Product.Image;
            result.Product.Name = product_detail.Product.Name;

            return result;
        }

        public Product findById(string id)
        {
            return _db.Products.ToList().SingleOrDefault(i => i.Id.Equals(id));
        }

        public ProductDetailDisplay findProductDetailDisplayByProductId(string id)
        {
            ProductDetailDisplay product_detail_display = new ProductDetailDisplay();

            // Tim ten, ma san pham, mainstone, substone, color, thumbnail, images va geomancy

            var product = _db.Products.ToList().SingleOrDefault(i => i.Id.Equals(id));

            product_detail_display.NAME = product.Name;
            product_detail_display.CODE = product.Id;

            product_detail_display.MAIN_STONE = product.MainStone.Name;
            product_detail_display.SUB_STONE = product.SubStone.Name;
            product_detail_display.COLOR = product.Color;
            product_detail_display.THUMB_NAIL = product.Image;
            product_detail_display.IMAGES = product.Images.ToList();
            product_detail_display.GEOMANCY = product.Geomancy.Name;

            var productDetail = _db.ProductDetails.ToList().Where(i => i.ProductId.Equals(id)).OrderBy(s => s.Size).FirstOrDefault();

            // Tim gia cua san pham

            double price = productDetail.ProductPrice.SalePrice;

            // Kiem tra san pham co duoc giam gia

            var isDiscount = _db.ProductDiscounts.ToList().SingleOrDefault(p => p.ValidUntil >= DateTime.Today && p.ProductId.Equals(id));

            if(isDiscount !=null)
            {
                product_detail_display.DISCOUNT_VALUE = (int)isDiscount.DiscountValue;
                product_detail_display.UNIT = isDiscount.DiscountUnit;
                product_detail_display.PRICE_AFTER_DISCOUNT = (price * (double)(100 - product_detail_display.DISCOUNT_VALUE) / (double)100); 
            }

            product_detail_display.PRICE = (int)price;
            return product_detail_display;
        }

        public List<OverViewProduct> findProductWithSameCategoryById(string id)
        {
            var relateProducts = new List<OverViewProduct>();
            var product = _db.Products.ToList().SingleOrDefault(i => i.Id.Equals(id));

            var relatedProducts = _db.Products.ToList().Where(i => i.CatId.Equals(product.CatId) && !(i.Id.Equals(product.Id))).ToList();

            foreach (var item in relatedProducts)
            {
                var productDetail = _db.ProductDetails.ToList().Where(i => i.ProductId.Equals(item.Id)).OrderBy(s => s.Size).FirstOrDefault();

                // Kiem tra san pham co duoc giam gia

                var isDiscount = _db.ProductDiscounts.ToList().SingleOrDefault(p => p.ValidUntil >= DateTime.Today && p.ProductId.Equals(item.Id));
                
                var relateProduct = new OverViewProduct();

                relateProduct.THUMB_NAIL = item.Image;
                relateProduct.NAME = item.Name;
                relateProduct.PRICE = productDetail.ProductPrice.SalePrice;
                relateProduct.CODE = item.Id;
                relateProduct.CATE_NAME = item.Cat.Name;

                if(isDiscount !=null)
                {
                    relateProduct.PRICE_AFTER_DISCOUNT = (double)productDetail.ProductPrice.SalePrice * (double)(100 - isDiscount.DiscountValue) / (double)100;
                }
                else
                {
                    relateProduct.PRICE_AFTER_DISCOUNT = 0;
                }

                relateProducts.Add(relateProduct);

            }

            return relateProducts;
        }

        public List<CustomerReview> findAllReviewById(string id)
        {
            var customerReviews = new List<CustomerReview>();

            var reviews = _db.Reviews.ToList().Where(r => r.ProductId.Equals(id)).OrderByDescending(i => i.Created_Date).ToList();

            foreach (var item in reviews)
            {
                var customerReview = new CustomerReview();
                customerReview.CONTENT = item.Content;
                customerReview.CREATED = (DateTime)item.Created_Date;
                customerReview.CUSTOMER_ID = item.CustomerId;
                customerReview.CUSTOMER_NAME = item.Customer.Fullname;
                customerReview.IS_UPDATE = (bool)item.Is_Update;
                customerReview.RATE = (int)item.Rate;
                customerReview.TITLE = item.Title;

                customerReviews.Add(customerReview);
            }

            return customerReviews;
        }
    }
}
