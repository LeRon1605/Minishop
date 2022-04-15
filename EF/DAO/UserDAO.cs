using EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class UserDAO
    {
        private ShopOnlineDbContext context;
        public UserDAO()
        {
            context = new ShopOnlineDbContext();
        }

        public User find(int id)
        {
            return context.Users.Find(id);
        }
        public List<User> findAll()
        {
            return context.Users.ToList();
        }
        public async Task<bool> Add(User newUser)
        {
            User user = context.Users.ToList().Where(u => u.Email == newUser.Email).FirstOrDefault();
            if (user != null)
            {
                return false;
            }
            else
            {
                newUser.CreatedAt = DateTime.Now;
                newUser.UpdatedAt = null;
                newUser.isActivated = false;
                newUser.RoleID = new RoleDAO().findByName("USER").ID;
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Role> GetUserRole(User user)
        {
            return await context.Roles.FindAsync(user.RoleID);
        }

        public async Task Activate(int id)
        {
            User user = context.Users.Find(id);
            user.isActivated = true;
            await context.SaveChangesAsync();
        }

        public User Login(string email, string password)
        {
            User user = context.Users.ToList().Where(u => (u.Email == email && u.Password == password)).FirstOrDefault();
            return user;
        }
    }
}
