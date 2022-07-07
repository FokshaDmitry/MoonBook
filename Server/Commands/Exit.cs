using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class Exit
    {
        AddDbContext add;
        Guid id;
        public Exit(Guid id)
        {
            this.id = id;
            add = new AddDbContext();
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                
                var q = add.Users.Where(u => u.Id == id).Single();
                q.Online = false;
                add.Users.Update(q);
                add.SaveChanges();
                response.code = LibProtocol.ResponseCode.Ok;
                response.succces = true;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Remove False";
            }
            return response;
        }
    }
}
