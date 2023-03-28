using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicCommerce.Areas.Customer.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/product")]
    public class ProductController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCate;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private IProductService _productService;

        public ProductController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , IProductService productService)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _productService = productService;
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
    }
}
