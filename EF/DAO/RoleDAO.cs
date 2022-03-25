using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class RoleDAO
    {
        private ShopOnlineDbContext context;
        public RoleDAO()
        {
            context = new ShopOnlineDbContext();
        }
        public List<Role> findAll()
        {
            return context.Roles.ToList();
        }
        public Role find(int id)
        {
            return context.Roles.Find(id);
        }
        public Role findByName(string Name)
        {
            return context.Roles.ToList().Where(role => role.Name == Name).FirstOrDefault();
        }
    }
}
