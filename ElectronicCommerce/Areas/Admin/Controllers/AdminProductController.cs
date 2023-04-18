using AspNetCoreHero.ToastNotification.Abstractions;
using Castle.Core.Configuration;
using ElectronicCommerce.Areas.Admin.Helpers;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Controllers
{

    [Area("admin")]
    [Route("admin/product")]


    public class AdminProductController : Controller
    {
        private IBaseRepository<Product> _baseRepoProduct;
        private IBaseRepository<CategoryProduct> _baseRepoCategory;
        private ICategoryProductService _categoryProductService;
        private IBaseRepository<StoneType> _baseRepoStoneType;
        private IBaseRepository<Geomancy> _baseRepoGeomancy;

        private INotyfService _notyfService;
        private IProductService _productService;
        private IBaseRepository<ProductDetail> _baseRepoProductDetail;
        private IBaseRepository<ProductPrice> _baseRepoProductPrice;


        public AdminProductController(IBaseRepository<Product> baseRepoProduct, INotyfService notyfService, IProductService productService,
            ICategoryProductService categoryProductService, IBaseRepository<StoneType> baseRepoStoneType,
            IBaseRepository<Geomancy> baseRepoGeomancy, IBaseRepository<CategoryProduct> baseRepoCategory,
             IBaseRepository<ProductDetail> baseRepoProductDetail, IBaseRepository<ProductPrice> baseRepoProductPrice)
        {
            _baseRepoProduct = baseRepoProduct;
            _notyfService = notyfService;
            _productService=productService;
            _categoryProductService=categoryProductService;
            _baseRepoStoneType = baseRepoStoneType;
            _baseRepoGeomancy = baseRepoGeomancy;
            _baseRepoCategory = baseRepoCategory;
            _baseRepoProductDetail = baseRepoProductDetail;
            _baseRepoProductPrice = baseRepoProductPrice;
        }


        [Route("index")]
        [Route("")]
        [Route("~/")]
        public IActionResult Index()
        {
            var product = _baseRepoProduct.GetAll().ToList();
            ViewBag.products = product;
            ViewBag.stoneTypes = _baseRepoStoneType.GetAll().ToList();
            ViewBag.Categories = _baseRepoCategory.GetAll().ToList().Where(i => i.ParentId != null).ToList();
            ViewBag.Geomancies = _baseRepoGeomancy.GetAll().ToList();
            return View("index",new Product());
        }

        [Route("updatebestseller")]
        [HttpPost]
        public IActionResult UpdateBestSeller(string product_id)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(product_id));
            if(product.BestSeller)
            {
                product.BestSeller = false;
            }
            else
            {
                product.BestSeller = true;
            }

            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();
            return new JsonResult(new {message="Success"});
        }

        [Route("updatehomeflag")]
        [HttpPost]
        public IActionResult UpdateHomeFlag(string product_id)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(product_id));
            if (product.HomeFlag)
            {
                product.HomeFlag = false;
            }
            else
            {
                product.HomeFlag = true;
            }

            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();
            return new JsonResult(new { message = "Success" });
        }

        [Route("updateactive")]
        [HttpPost]
        public IActionResult UpdateActive(string product_id)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(product_id));
            if (product.Active)
            {
                product.Active = false;
            }
            else
            {
                product.Active = true;
            }

            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();
            return new JsonResult(new { message = "Success" });
        }

        [Route("detail/{id}")]
        public IActionResult Detail(string id)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(id));
            ViewBag.products = product; 
            List<VMProductDetail> productDetail = _productService.findAllProduct().ToList().Where(i => i.ProductId.Equals(id)).ToList();
            ViewBag.productDetails = productDetail;
            ViewBag.stoneTypes = _baseRepoStoneType.GetAll().ToList();
            ViewBag.Categories = _baseRepoCategory.GetAll().ToList().Where(i => i.ParentId != null).ToList();
            ViewBag.Geomancies = _baseRepoGeomancy.GetAll().ToList();
            return View("detail");
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(VMProductDetail vM)
        {
            var product = _baseRepoProduct.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(vM.ProductId));
            product.Name = vM.ProductName;
            product.GeomancyId = vM.GeomancyId;
            product.MainStoneId = vM.MainStoneId;
            product.SubStoneId = vM.SubStoneId;
            product.Color = vM.Color;
            product.Note = vM.Note;
            product.CatId = vM.CategoryId;
            _baseRepoProduct.Update(product);
            _baseRepoProduct.Save();

            return RedirectToAction("detail", new { id=product.Id });
        }

        [HttpPost]
        [Route("addProductDetail")]
        public IActionResult addProductDetail(int size, int import_quantity, int base_price, int sale_price, string product_id)
		{
            ProductPrice productPrice = new ProductPrice();
            productPrice.ProductPriceId = "PP" + PrimarykeyHelper.RandomString(3);
            productPrice.BasePrice = base_price;
            productPrice.SalePrice = sale_price;
            productPrice.CreatedDate= DateTime.Now;
            productPrice.InActive = true;
            _baseRepoProductPrice.Insert(productPrice);
            _baseRepoProductPrice.Save();
            ProductDetail productDetail = new ProductDetail();
            productDetail.ProductDetailId = "PD" + PrimarykeyHelper.RandomString(3);
            productDetail.ProductId = product_id;
            productDetail.Quantity = import_quantity;
            productDetail.ImportQuantity = import_quantity;
            productDetail.ProductPriceId = productPrice.ProductPriceId;
            productDetail.CreatedDate = DateTime.Now;
            productDetail.Size = size;
            _baseRepoProductDetail.Insert(productDetail);
            _baseRepoProductDetail.Save();

            return RedirectToAction("detail", new { id=product_id });

        }


        [HttpPost]
        [Route("importQuantity")]
        public IActionResult importQuantity(string id, int import_quantity)
        {
            List<VMProductDetail> product = _productService.findAllProduct().ToList().Where(i => i.ProductDetailId.Equals(id)).ToList();
            string product_id = product[0].ProductId;
            var productDetail = _baseRepoProductDetail.GetAll().ToList().SingleOrDefault(p => p.ProductDetailId.Equals(id));
            productDetail.ImportQuantity = productDetail.ImportQuantity + import_quantity;
            productDetail.Quantity = productDetail.Quantity + import_quantity;
            _baseRepoProductDetail.Update(productDetail);
            _baseRepoProductDetail.Save();
            return RedirectToAction("detail", new { id = product_id });

        }

        [Route("deleteProductDetail/{id}")]
        public IActionResult deleteProductDetail(string id)
		{
            List<VMProductDetail> product = _productService.findAllProduct().ToList().Where(i => i.ProductDetailId.Equals(id)).ToList();
            string product_id = product[0].ProductId;
            _baseRepoProductDetail.Delete(id);
            _baseRepoProductDetail.Save();
            return RedirectToAction("detail", new { id = product_id});

        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(Product product)
		{
            product.Id = "SP"+ PrimarykeyHelper.RandomString(3);
            product.BestSeller = false;
            product.Active = false;
            product.HomeFlag = false;
            _baseRepoProduct.Insert(product);
            _baseRepoProduct.Save();
            return RedirectToAction("index");
		}
    }
}
