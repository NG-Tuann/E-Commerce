using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class OrderDetail
    {
        public string ProductId { get; set; }
        public string OrderId { get; set; }
        public int? Quantity { get; set; }

        public virtual OrderProduct Order { get; set; }
    }
}
