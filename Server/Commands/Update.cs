using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class Update
    {
        LibProtocol.Models.User data;
        AddDbContext add;

        public Update(LibProtocol.Models.User data)
        {
            this.data = data;
            add = new AddDbContext();
        }

        public bool ChekLogin()
        {
            if (add.Users.Count() != 0)
            {
                foreach (var item in add.Users)
                {
                    if (item.Login == data.Login)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                var q = add.Users.Where(u => u.Id == data.Id).Single();
                if(data.Password != "")
                {
                    q.Name = data.Name;
                    q.Surname = data.Surname;
                    q.Phpto = data.Phpto;
                    q.Password = data.Password;
                    q.Login = data.Login;
                    q.Status = data.Status;
                    q.DateOfBith = data.DateOfBith; 
                }
                else
                {
                    q.Name = data.Name;
                    q.Surname = data.Surname;
                    q.Phpto = data.Phpto;
                    q.Login = data.Login;
                    q.Status = data.Status;
                    q.DateOfBith = data.DateOfBith;
                }
                add.Update(q);
                add.SaveChanges();
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Update False";
            }
            return response;
        }
    }
}
