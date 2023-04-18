using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Route("customer/home")]
    public class HomeController : Controller
    {
        private IBaseRepository<CategoryProduct> _baseRepoCate;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;
        private IProductService _productService;

        public HomeController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , IProductService productService)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _productService = productService;
        }

        // GET: /<controller>/
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            ViewBag.homePros = _productService.findAllHomeFlagProducts();
            return View();
        }
    }
}
