using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class Login
    {
        LibProtocol.Models.User data;
        LibProtocol.Services.Hasher _hasher;
        AddDbContext add;
        public Login(LibProtocol.Models.User data)
        {
            this.data = data;
            add = new AddDbContext();
            _hasher = new LibProtocol.Services.Hasher();
        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                if (add.Users.Count() == 0)
                {
                    response.succces = false;
                    response.code = LibProtocol.ResponseCode.Error;
                    response.StatusTxt = "Database is Empty";
                    return response;

                }
                var item = add.Users.Where(u => u.Login == data.Login).FirstOrDefault();
                if (item != null)
                {
                    byte[]? img = null;
                    if (!String.IsNullOrEmpty(item.PhotoName))
                    {
                        img = File.ReadAllBytes("./img/" + item.PhotoName);
                    }
                    String PassSalt = _hasher.heshString(data.Password + item.PassSalt);
                    if (item.Login == data.Login && item.Password == PassSalt)
                    {
                        item.Online = true;
                        add.Users.Update(item);
                        response.data = new LibProtocol.Models.User { Id = item.Id, Name = item.Name, Surname = item.Surname, Phpto = img, Online = item.Online, Login = item.Login, DateOfBith = item.DateOfBith, Status = item.Status };
                        response.succces = true;
                        response.code = LibProtocol.ResponseCode.Ok;
                        response.StatusTxt = "Login Ok";
                    }
                    else
                    {
                        response.succces = false;
                        response.code = LibProtocol.ResponseCode.Error;
                        response.StatusTxt = "Wrong login or password";
                    }
                }
                else
                {
                    response.succces = false;
                    response.code = LibProtocol.ResponseCode.Error;
                    response.StatusTxt = "User don't found";
                }
                
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Wrong login or password";
            }
            
            add.SaveChanges();
            return response;
        }
    }
}
