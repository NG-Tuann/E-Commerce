using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class ProductDiscount
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int? DiscountValue { get; set; }
        public string DiscountUnit { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool Active { get; set; }

        public string Name { get; set; }

        public virtual Product Product { get; set; }
    }
}
