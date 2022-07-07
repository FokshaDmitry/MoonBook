using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class OnlineBook
    {
        AddDbContext add;
        LibProtocol.Online online;
        Guid id;
        List<Guid> IdP;
        public OnlineBook(Guid id)
        {
            this.id = id;
            online = new LibProtocol.Online();
            online.books = new List<LibProtocol.Models.Books>();
            online.subBook = new List<LibProtocol.Models.SubBook>();
            IdP = new List<Guid>();
            add = new AddDbContext();
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                foreach (var book in add.Books.Where(b => b.idUser == id))
                {
                    online.books.Add(book);
                }
                foreach (var subbook in add.SubBooks.Where(s => s.idUser == id).Join(add.Books, s => s.idBook, b => b.Id, (s, b) => new { Sub = s, Book = b }))
                {
                    online.subBook.Add(subbook.Sub);
                    online.books.Add(subbook.Book);
                }
                response.data = new LibProtocol.Online { books = online.books, subBook = online.subBook };
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Online False";
            }
            return response;
        }
    }
}
