using Models.DTO;
using Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL
{
    public class RoleBLL
    {
        private ShopOnlineDbContext context;
        public RoleBLL()
        {
            context = new ShopOnlineDbContext();
        }
        public List<Role> findAll()
        {
            return context.Roles.AsNoTracking().ToList();
        }
        public Role find(int id)
        {
            return context.Roles.Find(id);
        }
        public Role findByName(string Name)
        {
            return context.Roles.AsNoTracking().ToList().Where(role => role.Name == Name).FirstOrDefault();
        }
    }
}
