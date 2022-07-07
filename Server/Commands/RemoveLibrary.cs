using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class RemoveLibrary
    {
        LibProtocol.Models.SubBook data;
        AddDbContext add;
        public RemoveLibrary(LibProtocol.Models.SubBook data)
        {
            this.data = data;
            add = new AddDbContext();
        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                var q = add.SubBooks.Where(s => s.idUser == data.idUser).Where(s => s.idBook == data.idBook).Single();
                add.SubBooks.Remove(q);
                add.SaveChanges();
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Coment False";
            }
            return response;
        }
    }
}
