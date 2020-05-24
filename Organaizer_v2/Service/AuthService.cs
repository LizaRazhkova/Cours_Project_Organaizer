using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Organaizer_v2.Service
{
    public class AuthService
    {
        public Users User { get; private set; }

        public async Task<Users> AddUserInDB(string UserName, string Password)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                Users user = new Users()
                {
                    NameUser = UserName,
                    Password = Password
                };
                User = db.Users.Add(user);
                await db.SaveChangesAsync();
                return User;
            }
        }

        public bool ActiveUser(string UserName, string Password)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {

                //User activeUser = db.Users.FirstOrDefault(user => user.NameUser == UserName);
                IQueryable<Users> CollectionActiveUser = from item in db.Users where item.NameUser == UserName select item;
                if (CollectionActiveUser == null) return false;
                foreach (var activeUser in CollectionActiveUser) {
                    string passwordHash = activeUser.Password;
                    if (BCrypt.Net.BCrypt.Verify(Password, passwordHash))
                    {
                        User = activeUser;
                        return true;
                    }
                }

                return false;
            }
        }

        public async Task<Users> SettingChangedName(int id, string name)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                Users activeUser = db.Users.FirstOrDefault(user => user.id == id);
                if (activeUser != null)
                {
                    activeUser.NameUser = name;
                    User = activeUser;
                   await db.SaveChangesAsync();
                    return User;
                }
                else return null;
            }
        }

        public async Task<Users> SettingChangedPassword(int id, string oldpassword, string password)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                Users activeUser = db.Users.FirstOrDefault(user => user.id == id);
                string passwordHash = activeUser.Password;
                if (BCrypt.Net.BCrypt.Verify(oldpassword,passwordHash)) { 
                    password = BCrypt.Net.BCrypt.HashPassword(password);
                    activeUser.Password = password;
                    User = activeUser;
                    await db.SaveChangesAsync();
                    return User;
                }
                else return null;
            }
        }
    }
}
