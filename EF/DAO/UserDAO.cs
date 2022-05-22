using EF.DAO;
using Models.DTO;
using Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO_Test
{
    public class UserDAO: BaseDAO<User>
    {
        public bool Update(User entity)
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
}
