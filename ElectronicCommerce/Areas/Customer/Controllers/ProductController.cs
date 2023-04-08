using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Areas.Customer.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/product")]
    public class ProductController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCate;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private IBaseRepository<ProductDiscount> _baseProductDiscount;
        private IProductService _productService;

        public ProductController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , IProductService productService, IBaseRepository<ProductDiscount> baseProductDiscount)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _productService = productService;
            _baseProductDiscount = baseProductDiscount;
        }
        [Route("index")]
        [Route("")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("detail/{id}")]
        public IActionResult detail(string id)
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            var product_detail_display = _productService.findProductDetailDisplayByProductId(id);

            ViewBag.productDetail = product_detail_display;

            // Binh luan ve san pham cua customer

            ViewBag.customerReviews = _productService.findAllReviewById(id);

            // San pham cung dong

            ViewBag.relateProducts = _productService.findProductWithSameCategoryById(id);

            // Tao session luu nhung san pham da xem cua khach hang

            if (HttpContext.Session.GetString("viewed") == null)
            {
                var viewed = new List<OverViewProduct>();
                viewed.Add(new OverViewProduct
                {
                    CODE = product_detail_display.CODE,
                    THUMB_NAIL = product_detail_display.THUMB_NAIL,
                    NAME = product_detail_display.NAME,
                    PRICE = product_detail_display.PRICE,
                    PRICE_AFTER_DISCOUNT = product_detail_display.PRICE_AFTER_DISCOUNT,
                }) ;
                HttpContext.Session.SetString("viewed", JsonConvert.SerializeObject(viewed));
                var viewedProducts = JsonConvert.DeserializeObject<List<OverViewProduct>>(HttpContext.Session.GetString("viewed"));

                ViewBag.viewedProducts = viewedProducts;
            }
            else
            {
                var viewed = JsonConvert.DeserializeObject<List<OverViewProduct>>(HttpContext.Session.GetString("viewed"));

                bool isExists = isViewed(id, viewed);

                if (isExists)
                {
                    ViewBag.viewedProducts = viewed;
                }
                else
                {
                    viewed.Add(new OverViewProduct
                    {
                        CODE = product_detail_display.CODE,
                        THUMB_NAIL = product_detail_display.THUMB_NAIL,
                        NAME = product_detail_display.NAME,
                        PRICE = product_detail_display.PRICE,
                        PRICE_AFTER_DISCOUNT = product_detail_display.PRICE_AFTER_DISCOUNT,
                    });
                    viewed.Reverse();
                    HttpContext.Session.SetString("viewed", JsonConvert.SerializeObject(viewed));
                    var viewedProducts = JsonConvert.DeserializeObject<List<OverViewProduct>>(HttpContext.Session.GetString("viewed"));
                    ViewBag.viewedProducts = viewedProducts;
                }
            }

            return View("detail");
        }

        [HttpPost]
        [Route("findProductById")]
        public IActionResult findProductById(string product_id)
        {
            return new JsonResult(_productService.findProductById(product_id));
        }

        [HttpPost]
        [Route("findAllSizeOfProducts")]
        public IActionResult findAllSizeOfProducts(string product_id)
        {
            return new JsonResult(_productService.findAllSizeOfProducts(product_id));
        }

        [HttpPost]
        [Route("findPriceBySizeAndId")]
        public IActionResult findPriceBySizeAndId(int size, string product_id)
        {
            return new JsonResult(_productService.findPriceBySizeAndId(size, product_id));
        }

        [HttpGet]
        [Route("findAllCart")]
        public IActionResult findAllCart()
        {
            if (HttpContext.Session.GetString("cart") == null)
            {
                return new JsonResult(null);
            }
            else
            {
                var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                return new JsonResult(cartSession);
            }
        }

        [HttpPost]
        [Route("AddToCart")]
        public IActionResult AddToCart(int size, string product_id)
        {
            ProductDetail product_detail = _productService.findProductDetailByProductIdAndSize(size, product_id);

            var isDiscount = _baseProductDiscount.GetAll().ToList().SingleOrDefault(i => i.ProductId.Equals(product_detail.Product.Id));

            int discountValue = 0;

            if(isDiscount !=null)
            {
                discountValue = (int)isDiscount.DiscountValue;
            }

            int actualPrice = product_detail.ProductPrice.SalePrice;

            int savePrice = 0;

            double disPrice1 = (double)discountValue / (double)100;
            double disPrice2 = (double)(100 - discountValue) / (double)100;

            if (discountValue > 0)
            {
                savePrice = (int)(product_detail.ProductPrice.SalePrice * disPrice1);
                actualPrice = (int)(product_detail.ProductPrice.SalePrice * disPrice2);
            }

            // Tao session
            if (HttpContext.Session.GetString("cart") == null)
            {
                var cart = new List<Item>();
                cart.Add(new Item
                {
                    product = product_detail,
                    image = product_detail.Product.Image,
                    size = product_detail.Size,
                    name = product_detail.Product.Name,
                    quantity = 1,
                    price = actualPrice,
                    isCheck = true,
                    savePrice = savePrice
                }) ;
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                return new JsonResult(cartSession);
            }
            else
            {
                var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

                int index = Exists(product_detail.ProductDetailId, cart);

                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        product = product_detail,
                        image = product_detail.Product.Image,
                        size = product_detail.Size,
                        name = product_detail.Product.Name,
                        quantity = 1,
                        price = actualPrice,
                        isCheck = true,
                        savePrice = savePrice
                    });
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                }
                else
                {
                    if(cart[index].quantity == product_detail.Quantity)
                    {
                        return new JsonResult(new { message = "Outstock" });
                    }
                    cart[index].quantity++;
                    cart[index].savePrice += savePrice;
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                }

                var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
                return new JsonResult(cartSession);
            }
        }

        [Route("removeFromCart")]
        [HttpPost]
        public IActionResult removeFromCart(string product_detail_id)
        {
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            int index = Exists(product_detail_id, cart);
            cart.RemoveAt(index);

            if(cart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }

            return new JsonResult(new { message = "success" });
        }

        private int Exists(string id, List<Item> cart)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].product.ProductDetailId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool isViewed(string id, List<OverViewProduct> viewedProducts)
        {
            for (int i = 0; i < viewedProducts.Count; i++)
            {
                if (viewedProducts[i].CODE == id)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
