using System;
using System.Collections.Generic;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public interface IUserService
    {
        public List<User> findAllUserByRole(string role_id);
    }
}
