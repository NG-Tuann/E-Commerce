using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/cart")]
    public class CartController : Controller
    {
        private IBaseRepository<ProductDetail> _productDetailRepo;

        public CartController(IBaseRepository<ProductDetail> productDetailRepo)
        {
            _productDetailRepo = productDetailRepo;
        }
        // GET: /<controller>/
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("")]
        [Route("CartPage")]
        public IActionResult CartPage()
        {
            return View("cartpage");
        }

        [Route("BookingPage")]
        public IActionResult BookingPage()
        {
            return View("bookingpage");
        }

        [HttpPost]
        [Route("updateQuantity")]
        public IActionResult UpdateQuantity(string product_detail_id, string action)
        {
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            for (int i = 0; i < cart.Count; i++)
            {
                if(cart[i].product.ProductDetailId.Equals(product_detail_id))
                {
                    if(action.Equals("minus"))
                    {
                        cart[i].quantity -= 1;
                    }
                    else if(action.Equals("plus"))
                    {
                        if(cart[i].quantity == _productDetailRepo.GetById(product_detail_id).Quantity)
                        {
                            return new JsonResult(new { message = "Outstock" });
                        }
                        cart[i].quantity += 1;
                    }
                }
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return new JsonResult(new {message="Update quantity success"});
        }

        [HttpPost]
        [Route("updatePaidProduct")]
        public IActionResult UpdatePaidProduct(string product_detail_id, bool isCheck)
        {
            var cart = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].product.ProductDetailId.Equals(product_detail_id))
                {
                    cart[i].isCheck = isCheck;                    
                }
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return new JsonResult(new { message = "Update paid product success" });
        }
    }
}
