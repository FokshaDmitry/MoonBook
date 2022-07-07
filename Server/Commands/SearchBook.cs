using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class SearchBook
    {
        string data;
        LibProtocol.Online online;
        AddDbContext add;
        public SearchBook(String data)
        {
            this.data = data;
            add = new AddDbContext();
            online = new LibProtocol.Online();
            online.books = new List<LibProtocol.Models.Books>();
        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                foreach (var item in add.Books)
                {
                    if ($"{item.Title.Replace(" ", "").ToLower()}" == data.ToLower())
                    {
                        online.books.Add(item);
                    }
                }
                if (online.books.Count() == 0)
                {
                    response.succces = false;
                    response.code = LibProtocol.ResponseCode.Error;
                    response.StatusTxt = "No matvhes found";
                    return response;
                }
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Wrong Search";
                return response;
            }

            response.data = online;
            response.succces = true;
            response.code = LibProtocol.ResponseCode.Ok;
            return response;
        }
    }
}
