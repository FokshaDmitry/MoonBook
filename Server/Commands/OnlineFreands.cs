using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class OnlineFreands
    {
        Guid data;
        AddDbContext add;
        LibProtocol.Online online;
        public OnlineFreands(Guid data)
        {
            this.data = data;
            add = new AddDbContext();
            online = new LibProtocol.Online();
            online.users = new List<LibProtocol.Models.User>();
            online.subscriptions = new List<LibProtocol.Models.Subscriptions>();
        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                foreach (var item in add.Subscriptions.Where(s => s.IdUser == data).Join(add.Users, s => s.IdFreand, u => u.Id, (s, u) => new {Sub = s, User = u}))
                { 
                        online.users.Add(item.User);
                }
                response.data = online;
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Wrong Search";
                return response;
            }

            
            return response;
        }
    }
}
