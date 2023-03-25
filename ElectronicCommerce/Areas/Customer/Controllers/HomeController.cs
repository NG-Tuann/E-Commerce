using System;
using System.Collections.Generic;
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
        private IBaseRepository<CategoryProduct> _baseRepo;

        public HomeController(IBaseRepository<CategoryProduct> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        // GET: /<controller>/
        [Route("")]
        [Route("~/")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.cates = _baseRepo.GetAll().ToList();
            return View();
        }
    }
}
