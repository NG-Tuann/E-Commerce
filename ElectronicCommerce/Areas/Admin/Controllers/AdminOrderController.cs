using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Customer.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/order")]
    public class AdminOrderController : Controller
    {
        private IBaseRepository<OrderProduct> _baseOrderProduct;
        private INotyfService _notyfService;
        private IOrderProductService _orderProductService;

        public AdminOrderController(INotyfService notyfService, IBaseRepository<OrderProduct> baseOrderProduct
            ,IOrderProductService orderProductService)
        {
            _notyfService = notyfService;
            _baseOrderProduct = baseOrderProduct;
            _orderProductService = orderProductService;
        }

        // GET: /<controller>/
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.orders = _baseOrderProduct.GetAll().ToList();
            return View();
        }

        [Route("findCancelOrder")]
        public IActionResult findCancelOrder()
        {
            ViewBag.orders = _baseOrderProduct.GetAll().ToList().Where(i => i.OrderState.Equals("Đã yêu cầu huỷ đơn")).OrderByDescending(i => i.DateCreated).ToList();
            return View("index");
        }

        [HttpGet]
        [Route("confirmCancel/{id}")]
        public IActionResult confirmCancel(string id)
        {
            ConfirmCancel(id);
            return RedirectToAction("index");
        }

        [HttpGet]
        [Route("confirmOrder/{id}")]
        public IActionResult confirmOrder(string id)
        {
            ConfirmOrder(id);
            return RedirectToAction("index");
        }

        private void ConfirmCancel(string order_id)
        {
            var order = _baseOrderProduct.GetById(order_id);
            order.OrderState = "Đã huỷ đơn";
            _baseOrderProduct.Update(order);
            _baseOrderProduct.Save();

            // Cap nhat so luong cho san pham khi xac nhan huy don
            _orderProductService.UpdateQuantityCancelOrder(order);
        }

        private void ConfirmOrder(string order_id)
        {
            var order = _baseOrderProduct.GetById(order_id);
            order.OrderState = "Đã xác nhận";
            _baseOrderProduct.Update(order);
            _baseOrderProduct.Save();

            // Cap nhat so luong cho san pham khi xac nhan huy don
            _orderProductService.UpdateQuantityCancelOrder(order);
        }

        [HttpPost]
        [Route("filterOrderByDate")]
        public IActionResult filterOrderByDate(string start_date, string end_date)
        {
            DateTime sDate = DateTime.ParseExact(start_date, "dd/MM/yyyy",
                                         System.Globalization.CultureInfo.InvariantCulture);

            DateTime eDate = DateTime.ParseExact(end_date, "dd/MM/yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            var orders = _baseOrderProduct.GetAll().ToList().Where(i => i.DateCreated >= sDate && i.DateCreated <= eDate).ToList();
            ViewBag.orders = orders;
            return View("index");
        }


        [HttpPost]
        [Route("checkOrderStatus")]
        public IActionResult checkOrderStatus(string start_date, string end_date, string status)
        {
            DateTime sDate = DateTime.ParseExact(start_date, "dd/MM/yyyy",
                                         System.Globalization.CultureInfo.InvariantCulture);

            DateTime eDate = DateTime.ParseExact(end_date, "dd/MM/yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);

            var orders = _baseOrderProduct.GetAll().ToList().Where(i => i.DateCreated >= sDate && i.DateCreated <= eDate).ToList();

            orders.ForEach(i =>
            {
                i.OrderState = status;
                _baseOrderProduct.Update(i);
            });

            _baseOrderProduct.Save();

            ViewBag.orders = orders;
            return View("index");
        }

    }
}
