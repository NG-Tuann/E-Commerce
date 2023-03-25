using System;
using System.Collections.Generic;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public class UserService:IUserService
    {
        private IBaseRepository<User> _baseRepo;
        public UserService(IBaseRepository<User> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public List<User> findAllUserByRole(string role_id)
        {
            return _baseRepo.GetAll().ToList().Where(i => i.IdRole.Equals(role_id)).ToList();
        }
    }
}
