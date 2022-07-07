using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class AddLibrary
    {
        LibProtocol.Models.SubBook data;
        AddDbContext add;
        public AddLibrary(LibProtocol.Models.SubBook data)
        {
            this.data = data;
            add = new AddDbContext();
        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                add.SubBooks.Add(data);
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
