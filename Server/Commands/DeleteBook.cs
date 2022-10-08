using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class DeleteBook
    {
        AddDbContext add;
        Guid id;
        public DeleteBook(Guid id)
        {
            this.id = id;
            add = new AddDbContext();
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                var q = add.Books.Where(c => c.Id == id).FirstOrDefault();
                Guid deleteBook = Guid.NewGuid();
                add.DeleteLists.Add( new LibProtocol.Models.DeleteList { Id = deleteBook, Date = DateTime.Now, idElement = q.Id, idUser = (Guid)q.idUser } );
                q.Delete = deleteBook;
                add.Books.Update(q);
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
