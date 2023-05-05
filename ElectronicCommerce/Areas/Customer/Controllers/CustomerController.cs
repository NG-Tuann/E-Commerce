using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Diagnostics;
using ElectronicCommerce.Areas.Customer.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ElectronicCommerce.Areas.Customer.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/customer")]
    public class CustomerController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCate;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private ICustomerService _customerService;

        public CustomerController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , ICustomerService customerService)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _customerService = customerService;
        }
        // GET: /<controller>/
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("order")]
        public IActionResult Order()
        {
            var customer = ElectronicCommerce.Models.Customer.JsonDeserializeToCustomer(HttpContext.Session.GetString("customer"));
            var orderOfCustomer = _customerService.findAllOrderProductOfCustomer(customer.Id);
            if(orderOfCustomer.Count > 0 && orderOfCustomer !=null )
            {
                ViewBag.orders = orderOfCustomer;
            }
            return View("order");
        }

        // Login page
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();
            return View("Login");
        }

        [Route("GoogleLogin")]
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("googleresponse")
            });
        }

        [HttpPost]
        [Route("findAllPromotionsByCustomerId")]
        public IActionResult findAllPromotionsByCustomerId(string customer_id)
        {
            var result = _customerService.findAllPromotionsOfCustomer(customer_id);
            return new JsonResult(result);
        }

        [Route("googleresponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities
                        .FirstOrDefault().Claims.Select(claim => new
                        {
                            claim.Issuer,
                            claim.OriginalIssuer,
                            claim.Type,
                            claim.Value
                        });
            //Mail cua khach hang thanh vien
            string customerGmail = claims.ElementAt(4).Value;
            Debug.WriteLine("Gmail cua customer: "+ customerGmail);

            // Goi dich vu khach hang dang nhap bang google
            var customer = _customerService.signInWithGoogle(customerGmail);

            return RedirectToAction("loginsuccess", new {message = "success", customerId = customer.Id});
        }

        [HttpGet]
        [Route("loginsuccess")]
        public IActionResult Login(string? message,string? customerId)
        {
            var customer = _customerService.findCustomerById(customerId);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (message.Length >0 || message !=null)
            {
                if(message.Equals("success") && customer !=null)
                {
                    // Login thanh cong tao session

                    var signedCustomer = new ElectronicCommerce.Models.Customer();
                    signedCustomer.Id = customer.Id;
                    signedCustomer.Username = customer.Username;

                    HttpContext.Session.SetString("customer", ElectronicCommerce.Models.Customer.ToJson(signedCustomer));

                    // KHACH HANG CHECK
                    // Cap nhat gio hang cho khach hang neu co
                    // Neu truoc do ton tai session gio hang thi sau khi khach hang dang nhap
                    // Thuc hien cap nhat gio hang vao cart trong db
                    if(HttpContext.Session.GetString("cart") !=null)
                    {
                        var cartSession = JsonConvert.DeserializeObject<List<Item>>(HttpContext.Session.GetString("cart"));

                        // Goi dich vu cap nhat session va gio hang
                        _customerService.updateCartInDb(cartSession, customer);

                        // Do dbcart cua customer vao session cart
                        cartSession = _customerService.findAllCartByCustomerId(customer.Id);
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartSession, Formatting.Indented, settings));
                    }
                    else
                    {
                        // Do dbcart cua customer vao session cart
                        var cartSession = _customerService.findAllCartByCustomerId(customer.Id);
                        if(cartSession.Count >0)
                        {
                            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartSession, Formatting.Indented, settings));
                        }
                    }

                    // Kiem tra shipping_info session co duoc tao chua
                    // Neu co
                    if (HttpContext.Session.GetString("shipping_info") != null)
                    {
                        // Neu khach hang chua co thong tin
                        if(!(_customerService.isCustomerFilledInfo(customer.Id)))
                        {
                            HttpContext.Session.Remove("shipping_info");
                        }
                    }
                    // Neu chua
                    else
                    {
                        // Kiem tra xem khach hang co thong tin giao nhan hay khong
                        // Neu co thi tao session shipping_info do du lieu tu db len
                        if (_customerService.isCustomerFilledInfo(customerId))
                        {
                            var shippingInfo = new ShippingInformation();

                            shippingInfo.FULLNAME = customer.Fullname;
                            shippingInfo.MAIL = customer.Username;
                            shippingInfo.PHONE = customer.Phone;
                            shippingInfo.ADDRESS = customer.Address;

                            HttpContext.Session.SetString("shipping_info", JsonConvert.SerializeObject(shippingInfo));
                        }
                    }

                    return RedirectToAction("index", "home");
                }
                else
                {
                    return View("login");
                }
            }
            else
            {
                return View("pagenotfound");
            }
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            // Xoa session customer
            if (HttpContext.Session.GetString("customer") !=null)            {
                HttpContext.Session.Remove("customer");
            }

            // Xoa session shipinfo
            if (HttpContext.Session.GetString("shipping_info") != null)
            {
                HttpContext.Session.Remove("shipping_info");
            }

            // Xoa session cart

            if (HttpContext.Session.GetString("cart") != null)
            {
                HttpContext.Session.Remove("cart");
            }

            await HttpContext.SignOutAsync();
            return RedirectToAction("index","home");
        }
    }
}
