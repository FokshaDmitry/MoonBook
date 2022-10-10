using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandServer
{
    public class Online
    {
        AddDbContext add;
        LibProtocol.Online online;
        Guid id;
        List<Guid> IdP;
        public Online(Guid id)
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
                foreach (var Online in add.Posts.Where(p => p.IdUser == id).Where(p => p.Delete == Guid.Empty).OrderByDescending(p => p.Date).Join(add.Users, p => p.IdUser, u => u.Id, (p, u) => new { Post = p, User = u }))
                {
                    if (!String.IsNullOrEmpty(Online.User.PhotoName))
                    {
                        Online.User.Phpto = File.ReadAllBytes("./img/" + Online.User.PhotoName);
                    }
                    if (!String.IsNullOrEmpty(Online.Post.Image))
                    {
                        Online.Post.ImageMass = File.ReadAllBytes("./img_post/" + Online.Post.Image);
                    }
                    online?.posts?.Add(Online.Post);
                    online?.users?.Add(Online.User);
                    IdP.Add(Online.Post.Id);
                }
                foreach (var id in IdP)
                {
                    foreach (var comment in add.Comments.Where(c => c.idPost == id).Where(c => c.Delete == Guid.Empty))
                    {
                        online?.comments?.Add(comment);
                    }
                }
                foreach (var book in add.Books.Where(b => b.idUser == id).Where(b => b.Delete == Guid.Empty))
                {
                    if (!String.IsNullOrEmpty(book.CoverName))
                    {
                        book.CoverImage = File.ReadAllBytes("./img_post/" + book.CoverName);
                    }
                    online?.books?.Add(book);
                }
                foreach (var users in add.Subscriptions.Where(s => s.IdUser == id).Join(add.Users, s => s.IdFreand, u => u.Id, (s, u) => new {Sub = s, User = u}).Where(u => u.User.Delete == Guid.Empty))
                {
                    if (!String.IsNullOrEmpty(users.User.PhotoName))
                    {
                        users.User.Phpto = File.ReadAllBytes("./img/" + users.User.PhotoName);
                    }
                    online?.subscriptions?.Add(users.Sub);
                    online?.users?.Add(users.User);
                }
                response.data = new LibProtocol.Online { posts = online.posts, comments = online.comments, users = online.users, books = online.books, subscriptions = online.subscriptions };
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception ex)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Online False: " + ex.ToString();
            }
            return response;
        }
    }
}
