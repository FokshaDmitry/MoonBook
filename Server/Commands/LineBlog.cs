using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    public class LineBlog
    {
        AddDbContext add;
        LibProtocol.Online online;
        Guid id;
        List<Guid> IdP;
        List<Guid> IdU;
        public LineBlog(Guid id)
        {
            this.id = id;
            online = new LibProtocol.Online();
            online.posts = new List<LibProtocol.Models.Posts>();
            online.users = new List<LibProtocol.Models.User>();
            online.comments = new List<LibProtocol.Models.Comments>();
            IdP = new List<Guid>();
            IdU = new List<Guid>();
            add = new AddDbContext();
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                var q = add.Users.Where(u => u.Id == id).Single();
                online.users.Add(q);
                IdU.Add(q.Id);
                foreach (var Sub in add.Subscriptions.Where(s => s.IdUser == id).Join(add.Users, s => s.IdFreand, u => u.Id, (s, u) => new {freand = s, user = u}))
                {
                    online.users.Add(Sub.user);
                    IdU.Add(Sub.freand.IdFreand);
                }
                foreach (var User in IdU)
                {
                    foreach (var Post in add.Posts.Where(p => p.IdUser == User).OrderByDescending(p => p.Date))
                    {
                        online.posts.Add(Post);
                        IdP.Add(Post.Id);
                    }
                }
                foreach (var id in IdP)
                {
                    foreach (var comment in add.Comments.Where(c => c.idPost == id).Join(add.Users, c => c.idUser, u => u.Id, (c, u) => new {Com = c, user = u}))
                    {
                        online.comments.Add(comment.Com);
                        online.users.Add(comment.user);
                    }
                }
                response.data = new LibProtocol.Online { posts = online.posts, comments = online.comments, users = online.users };
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
