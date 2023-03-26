using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private IBaseRepository<Product> _baseRepoProduct;
        private DatabaseContext _db;

        public HomeController(IBaseRepository<CategoryProduct> baseRepoCate, IBaseRepository<Geomancy> baseRepoGeomancy
            , IBaseRepository<Product> baseRepoProduct, DatabaseContext db)
        {
            _baseRepoCate = baseRepoCate;
            _baseRepoGeomancy = baseRepoGeomancy;
            _baseRepoProduct = baseRepoProduct;
            _db = db;
        }

        // GET: /<controller>/
        [Route("")]
        [Route("~/")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.cates = _baseRepoCate.GetAll().ToList();
            ViewBag.geos = _baseRepoGeomancy.GetAll().ToList();

            ViewBag.homePros = _baseRepoProduct.GetAll().ToList().Where(i => i.HomeFlag == true).ToList();
            var homeProducts = (
                                from p in _db.Set<Product>()
                                join pd in _db.Set<ProductDetail>()
                                on p.Id equals pd.ProductId
                                join pc in _db.Set<ProductPrice>()
                                on pd.ProductPriceId equals pc.ProductPriceId
                                where pc.InActive == false && p.HomeFlag == true
                                select new { p.Name, pc.SalePrice, p.Image, pd.Size }
                                ).All(s => s.Size <=  (from pd1 in _db.Set<ProductDetail>()
                                                      join p1 in _db.Set<Product>()
                                                      on pd1.ProductId equals p1.Id
                                                      select pd1.Size).First());

            Debug.WriteLine(homeProducts);
            return View();
        }
    }
}
