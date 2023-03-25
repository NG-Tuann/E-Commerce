using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Cart
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public string OrderId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual OrderProduct Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
