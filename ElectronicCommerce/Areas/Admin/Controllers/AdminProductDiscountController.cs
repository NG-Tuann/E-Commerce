using ElectronicCommerce.Areas.Admin.Helpers;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/productdiscount")]
    public class AdminProductDiscountController : Controller
    {
        IBaseRepository<ProductDiscount> _baseRepoProductDiscount;
        IBaseRepository<Geomancy> _baseRepoGeomancy;
        IBaseRepository<StoneType> _baseRepoStoneType;

        public AdminProductDiscountController(IBaseRepository<ProductDiscount> baseRepoProductDiscount, IBaseRepository<Geomancy> baseRepoGeomancy,
            IBaseRepository<StoneType> baseRepoStoneType)
        {
            _baseRepoProductDiscount = baseRepoProductDiscount;
            _baseRepoGeomancy = baseRepoGeomancy;
            _baseRepoStoneType = baseRepoStoneType; 
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            // hien thi danh sach khuyen mai
            HashSet<string> visitedIds = new HashSet<string>();
            List<ProductDiscount> productDiscount = new List<ProductDiscount>();
            foreach (var item in _baseRepoProductDiscount.GetAll().ToList())
            {
                if (!visitedIds.Contains(item.Id))
                {
                    productDiscount.Add(item);
                    visitedIds.Add(item.Id);
                }
            }

            ViewBag.productDiscounts = productDiscount;

            //hiển thị danh sách đá và mệnh
            List<Geomancy> geomancy = _baseRepoGeomancy.GetAll().ToList();
            List<StoneType> stoneType = _baseRepoStoneType.GetAll().ToList();
            ViewBag.stoneTypes = stoneType;
            ViewBag.geomancies = geomancy;
            return View("index", new ProductDiscount());
        }


        [Route("detail/{id}")]
        public IActionResult Detail(string id)
        {
            List<ProductDiscount> productDiscounts = _baseRepoProductDiscount.GetAll().ToList().Where(p => p.Id.Equals(id)).ToList();
            ViewBag.productDiscounts = productDiscounts[0];

            return View("detail");
        }



        [HttpPost]
        [Route("add")]
        public IActionResult Add()
        {
            return RedirectToAction("index");

        }
    }
}
