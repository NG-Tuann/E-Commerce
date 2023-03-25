﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            Images = new HashSet<Image>();
            ProductDiscounts = new HashSet<ProductDiscount>();
            ProductPrices = new HashSet<ProductPrice>();
            Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public int? Size { get; set; }
        public int? ImportQuantity { get; set; }
        public string GeomancyId { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public bool? BestSeller { get; set; }
        public bool? HomeFlag { get; set; }
        public bool? Active { get; set; }
        public string CatId { get; set; }
        public string MainStoneId { get; set; }
        public string SubStoneId { get; set; }

        public virtual CategoryProduct Cat { get; set; }
        public virtual Geomancy Geomancy { get; set; }
        public virtual StoneType MainStone { get; set; }
        public virtual StoneType SubStone { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
