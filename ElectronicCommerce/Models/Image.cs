using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public string NameImages { get; set; }
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
