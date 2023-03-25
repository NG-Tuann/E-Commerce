using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Carts = new HashSet<Cart>();
            OrderProducts = new HashSet<OrderProduct>();
            PromotionDetails = new HashSet<PromotionDetail>();
            Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? Dob { get; set; }
        public string IdCard { get; set; }
        public string CustomerTypeId { get; set; }
        public int? ScorePay { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual CustomerType CustomerType { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<PromotionDetail> PromotionDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
