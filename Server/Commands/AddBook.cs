using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class AddBook
    {
        LibProtocol.Models.Books data;
        AddDbContext add;
        public AddBook(LibProtocol.Models.Books data)
        {
            this.data = data;
            add = new AddDbContext();
        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                add.Books.Add(data);
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
