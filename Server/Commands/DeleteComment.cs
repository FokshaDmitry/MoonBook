using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class DeleteComment
    {
        AddDbContext add;
        Guid id;
        public DeleteComment(Guid id)
        {
            this.id = id;
            add = new AddDbContext();
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                var q = add.Comments.Where(c => c.Id == id).Single();
                add.Remove(q);
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
