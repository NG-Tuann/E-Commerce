using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicCommerce.Areas.Customer.Models
{
    public class OverViewProductHomeFlag
    {
        public string PRODUCT_ID { get; set; }
        public string IMAGE { get; set; }
        public string NAME { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public int PRICE { get; set; }
    }
}
