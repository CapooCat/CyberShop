using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data
{
    public class UserDao
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public bool KTTaiKhoan(string tk)
        {
            var res = data.Users.Where(x => x.Username == tk).Count();
            if(res>0)
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
            var res = data.Users.Where(x => x.Username == email).Count();
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool KTMatKhau(string mk)
        {
            var res = data.Users.Where(x => x.Password == mk).Count();
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
    }
}
