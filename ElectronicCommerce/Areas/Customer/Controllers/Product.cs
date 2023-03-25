﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/product")]
    public class Product : Controller
    {
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
            return View("detail");
        }
    }
}