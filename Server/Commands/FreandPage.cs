using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class FreandPage
    {
        AddDbContext add;
        LibProtocol.Online online;
        Guid id;
        List<Guid> IdP;
        public FreandPage(Guid id)
        {
            this.id = id;
            online = new LibProtocol.Online();
            online.posts = new List<LibProtocol.Models.Posts>();
            online.users = new List<LibProtocol.Models.User>();
            online.comments = new List<LibProtocol.Models.Comments>();
            online.books = new List<LibProtocol.Models.Books>();
            online.subscriptions = new List<LibProtocol.Models.Subscriptions>();    
            IdP = new List<Guid>();
            add = new AddDbContext();
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                online.users.Add(add.Users.Where(u => u.Id == id).Single());
                foreach (var Online in add.Posts.Where(p => p.IdUser == id).OrderByDescending(p => p.Date))
                {
                    online.posts.Add(Online);
                    IdP.Add(Online.Id);
                }
                foreach (var id in IdP)
                {
                    foreach (var comment in add.Comments.Where(c => c.idPost == id).Join(add.Users, c => c.idUser, u => u.Id, (c, u) => new { Com = c, user = u }))
                    {
                        online.comments.Add(comment.Com);
                        online.users.Add(comment.user);
                    }
                }
                foreach (var book in add.Books.Where(b => b.idUser == id))
                {
                    online.books.Add(book);
                }
                foreach (var users in add.Subscriptions.Where(s => s.IdUser == id).Join(add.Users, s => s.IdFreand, u => u.Id, (s, u) => new {Sub = s, User = u}))
                {
                    online.subscriptions.Add(users.Sub);
                    online.users.Add(users.User);   
                }
                response.data = new LibProtocol.Online { posts = online.posts, comments = online.comments, users = online.users, subscriptions = online.subscriptions, books = online.books};
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Freand Page False";
            }
            return response;
        }
    }
}
