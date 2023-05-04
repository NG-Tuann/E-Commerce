using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

#nullable disable

namespace ElectronicCommerce.Models
{
    [Serializable]
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

        public static Customer JsonDeserializeToCustomer(string json)
        {
            var customer = new Customer();
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.LineNumber == 2 && reader.Value.ToString() != "id")
                {
                    customer.Id = reader.Value.ToString();
                }
                else if (reader.LineNumber == 3 && reader.Value.ToString() != "username")
                {
                    customer.Username = reader.Value.ToString();
                }
            }
            return customer;
        }

        public static string ToJson(Customer customer)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(customer.Id);

                writer.WritePropertyName("username");
                writer.WriteValue(customer.Username);

                writer.WriteEndObject();
            }

            return sw.ToString();
        }
    }
}
