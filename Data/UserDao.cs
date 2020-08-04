using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
namespace Data
{
    public class UserDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool KTTaiKhoan(string tk)
        {
            var res = data.Users.Where(x => x.Username == tk && x.IsDeleted == false).Count();
            if(res>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsAdmin(string tk)
        {
            var res = data.Users.Where(x => x.Username == tk && x.IsDeleted == false).First().UserType;
            if (res == "admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool NotAdmin(string tk)
        {
            var res = data.Users.Where(x => x.Username == tk && x.IsDeleted == false).First().UserType;
            if (res != "admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool KTEmail(string email)
        {
            var res = data.Users.Where(x => x.Username == email && x.IsDeleted == false).Count();
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool KTMatKhau(string mk, string tk)
        {
            var res = data.Users.Where(x => x.Password == mk && x.Username == tk && x.IsDeleted == false).Count();
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public User getInfo(string tk)
        {
            return data.Users.FirstOrDefault(x => x.Username == tk);
        }
        public bool InsertUser(User entity)
        {
            if (entity != null)
            {
                data.Users.Add(entity);
                data.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdateUser(User entity)
        {
            if(entity!=null)
            {
                var res = data.Users.Find(entity.id);
                res.Name = entity.Name;
                res.Address = entity.Address;
                res.PhoneNum = entity.PhoneNum;
                data.SaveChanges();
                return true;
            }
            return false;
        }
        public bool InsertCustomerOrDer(User entity)
        {
            try
            {
                if (entity != null)
                {
                    data.Users.Add(entity);
                    data.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
