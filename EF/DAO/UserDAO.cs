using EF.Helper;
using EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class UserDAO: IDAO<User>
    {
        public bool Delete(int id)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.Find(id);
                if (user == null) return false;
                else
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                    return true;
                }
            }    
        }

        public User find(int id)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Users.Find(id);
            }    
        }
        public User findByEmail(string email)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Users.FirstOrDefault(user => user.Email == email);
            }    
        }
        public List<User> findAll()
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Users.AsNoTracking().ToList();
            }
        }

        public void add(User user)
        {

        }
        public async Task<bool> Add(User newUser, bool isAdmin = false)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Email == newUser.Email);
                if (user != null)
                {
                    return false;
                }
                else
                {
                    if (isAdmin) newUser.RoleID = new RoleDAO().findByName("ADMIN").ID;
                    else
                    {
                        newUser.RoleID = new RoleDAO().findByName("USER").ID;
                        newUser.Cart = new Cart();
                    }
                    newUser.Image = $"/public/images/Default.jpg";
                    newUser.CreatedAt = DateTime.Now;
                    newUser.UpdatedAt = null;
                    newUser.isActivated = false;
                    await context.Users.AddAsync(newUser);
                    await context.SaveChangesAsync();
                    return true;
                }
            }         
        }

        public async Task<Role> GetUserRole(User user)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return await context.Roles.FindAsync(user.RoleID);
            }
        }

        public async Task Activate(int id)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.Find(id);
                user.isActivated = true;
                await context.SaveChangesAsync();
            }
        }

        public User Login(string email, string password)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.AsNoTracking().FirstOrDefault(u => (u.Email == email && u.Password == password));
                return user;
            }
        }

        public User Update(User entity)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.Find(entity.ID);
                if (user == null) return null;
                else
                {
                    user.Name = entity.Name;
                    user.Phone = entity.Phone;
                    user.Birth = entity.Birth;
                    user.Gender = entity.Gender;
                    user.Address = entity.Address;
                    user.Image = entity.Image;
                    if (user.Email != entity.Email)
                    {
                        user.Email = entity.Email;
                        user.isActivated = false;
                    }
                    user.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                    return find(user.ID);
                }
            }
        }

        public string ResetPasssword(string email)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null) return null;
                else
                {
                    string password = new Random().Next(10000000, 99999999).ToString();
                    user.Password = Encryptor.MD5Hash(password);
                    user.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                    return password;
                }
            }    
        }
    }
}
