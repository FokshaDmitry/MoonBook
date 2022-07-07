using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class UnSubscription
    {
        LibProtocol.Models.Subscriptions data;
        AddDbContext add;
        public UnSubscription(LibProtocol.Models.Subscriptions data)
        {
            this.data = data;
            add = new AddDbContext();

        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                foreach (var item in add.Subscriptions.Where(s => s.IdUser == data.IdUser && s.IdFreand == data.IdFreand))
                {
                    add.Subscriptions.Remove(item);
                } 
                add.SaveChanges();
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Delete False";
            }
            return response;
        }
    }
}
