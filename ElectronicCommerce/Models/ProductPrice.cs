using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class ProductPrice
    {
        public string ProductId { get; set; }
        public int Size { get; set; }
        public int? BasePrice { get; set; }
        public int? SalePrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InActive { get; set; }

        public virtual Product Product { get; set; }
    }
}
