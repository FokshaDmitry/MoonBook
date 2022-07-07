using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class ChekBook
    {
        AddDbContext add;
        LibProtocol.Models.SubBook data;
        bool chek;
        public ChekBook(LibProtocol.Models.SubBook data)
        {
            this.data = data;
            add = new AddDbContext();
            chek = false;
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                foreach (var book in add.SubBooks.Where(s => s.idUser == data.idUser))
                {
                    if (book.idBook == data.idBook)
                    {
                        chek = true;
                        response.data = chek;
                        response.code = LibProtocol.ResponseCode.Ok;
                        response.succces = true;
                        return response;
                    }
                }
                response.data = chek;
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
