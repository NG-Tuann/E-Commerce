using System;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Customer.Models
{
    public class Item
    {
        public ProductDetail product { get; set; }
        public string image { get; set; }
        public int size { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public bool isCheck { get; set; }
        public int? savePrice { get; set; }
    }
}
