using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicCommerce.Areas.Customer.Models
{
    public class OverViewProductHomeFlag
    {
        public string PRODUCT_ID { get; set; }
        public string IMAGE { get; set; }
        public string NAME { get; set; }
        public int PRICE { get; set; }
    }
}
