using System;
using System.Collections.Generic;
using ElectronicCommerce.Areas.Customer.Models;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public interface IOrderProductService
    {
        public string NonCustomerPayPalSuccess(List<Item> cart, ShippingInformation shipInfo);

        public string NonCustomerCodSuccess(List<Item> cart, ShippingInformation shipInfo);

        public string CustomerPayPalSuccess(List<Item> cart, ShippingInformation shipInfo, ElectronicCommerce.Models.Customer customer, string? code);

        public string CustomerCodSuccess(List<Item> cart, ShippingInformation shipInfo);
    }
}
