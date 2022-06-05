using EF.Helper;
using Models.DTO;
using Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL
{
    public class UserBO
    { 
        public List<User> getPage(int page, int pageSize, string keyword, out int totalRow)
        {
            totalRow = 0;
            if (page > 0)
            {
                using (ShopOnlineDbContext context = new ShopOnlineDbContext())
                {
                    totalRow = (int)Math.Ceiling(context.Users.Count() / (double)pageSize);
                    return context.Users.Select(user => new User {
                        ID = user.ID,
                        Name = user.Name,
                        Email = user.Email,
                        Birth = user.Birth,
                        Address = user.Address,
                        Gender = user.Gender,
                        Phone = user.Phone,
                        isActivated = user.isActivated,
                        Image = user.Image,
                        CreatedAt = user.CreatedAt,
                        Role = user.Role,
                        UpdatedAt = user.UpdatedAt
                    })
                        .Where(user => user.Role.Name == "USER" && (user.ID.ToString().Contains(keyword) || user.Name.Contains(keyword) || user.Email.Contains(keyword)))
                        .Skip((page - 1) * pageSize).Take(pageSize)
                        .ToList();
                }    
            }
            return null;
        }
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
                return context.Users.Select(user => new User 
                { 
                    ID = user.ID,
                    Name = user.Name, 
                    Email = user.Email,
                    Birth = user.Birth,
                    Address = user.Address,
                    Gender = user.Gender,
                    Phone = user.Phone,
                    isActivated = user.isActivated,
                    Image = user.Image,
                    CreatedAt = user.CreatedAt,
                    RoleID = user.RoleID,
                    UpdatedAt = user.UpdatedAt
                }).FirstOrDefault(user => user.ID == id);
            }    
        }
        public User findByEmail(string email)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Users.AsNoTracking().Select(user => new User
                {
                    ID = user.ID,
                    Name = user.Name,
                    Email = user.Email,
                    Birth = user.Birth,
                    Address = user.Address,
                    Gender = user.Gender,
                    Phone = user.Phone,
                    isActivated = user.isActivated,
                    Image = user.Image,
                    CreatedAt = user.CreatedAt,
                    RoleID = user.RoleID,
                    UpdatedAt = user.UpdatedAt
                }).FirstOrDefault(user => user.Email == email);
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
                    if (isAdmin) newUser.RoleID = new RoleBO().findByName("ADMIN").ID;
                    else
                    {
                        newUser.RoleID = new RoleBO().findByName("USER").ID;
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
                return context.Users.AsNoTracking().FirstOrDefault(u => (u.Email == email && u.Password == password));               
            }
        }

        public bool Update(User entity)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.Find(entity.ID);
                if (user == null) return false;
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
                    return true;
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

        public bool ChangePassword(string oldPassword, string newPassword, int ID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                User user = context.Users.FirstOrDefault(u => u.ID == ID);
                if (user == null) return false;
                else
                {
                    if (Encryptor.MD5Hash(oldPassword) == user.Password)
                    {
                        user.Password = Encryptor.MD5Hash(newPassword);
                        user.UpdatedAt = DateTime.Now;
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
