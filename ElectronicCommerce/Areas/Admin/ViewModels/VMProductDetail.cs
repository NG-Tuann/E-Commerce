using ElectronicCommerce.Models;
using System;
using System.Collections.Generic;

namespace ElectronicCommerce.Areas.Admin.ViewModels
{
    public class VMProductDetail
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string GeomancyId { get; set; }
        public string GeomancyName { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string MainStoneId { get; set; }
        public string MainStoneName { get; set; }
        public string SubStoneId { get; set; }
        public string SubStoneName { get; set; }
        public string ProductDetailId { get; set; }
        public int Quantity { get; set; }
        public int ImportQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Size { get; set; }
        public string ProductPriceId { get; set; }
        public int BasePrice { get; set; }
        public int SalePrice { get; set; }
        public DateTime CreatedDatePrice { get; set; }
        public bool InActivePrice { get; set; }


    }
}
