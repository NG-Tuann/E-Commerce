using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class ProductDetail
    {
        public string ProductDetailId { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public int? ImportQuantity { get; set; }
        public string ProductPriceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Size { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductPrice ProductPrice { get; set; }
    }
}
