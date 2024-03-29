﻿using Server.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
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
                var q = add.Users.Where(u => u.Id == id).FirstOrDefault();
                if (!String.IsNullOrEmpty(q?.PhotoName))
                {
                    q.Phpto = File.ReadAllBytes("./img/" + q?.PhotoName);
                }
                online?.users?.Add(q);
                IdU.Add(q.Id);
                foreach (var Sub in add.Subscriptions.Where(s => s.IdUser == id).Join(add.Users, s => s.IdFreand, u => u.Id, (s, u) => new {freand = s, user = u}).Where(u => u.user.Delete == Guid.Empty))
                {
                    if (!String.IsNullOrEmpty(Sub.user?.PhotoName))
                    {
                        Sub.user.Phpto = File.ReadAllBytes("./img/" + Sub.user?.PhotoName);
                    }
                    online?.users?.Add(Sub.user);
                    IdU.Add(Sub.freand.IdFreand);
                }
                foreach (var User in IdU)
                {
                    foreach (var Post in add.Posts.Where(p => p.IdUser == User).Where(p => p.Delete == Guid.Empty).OrderByDescending(p => p.Date))
                    {
                        if (!String.IsNullOrEmpty(Post.Image))
                        {
                            Post.ImageMass = File.ReadAllBytes("./img_post/" + Post.Image);
                        }
                        online?.posts?.Add(Post);
                        IdP.Add(Post.Id);
                    }
                }
                foreach (var id in IdP)
                {
                    foreach (var comment in add.Comments.Where(c => c.idPost == id).Where(c => c.Delete == Guid.Empty).Join(add.Users, c => c.idUser, u => u.Id, (c, u) => new {Com = c, user = u}))
                    {
                        if (!String.IsNullOrEmpty(comment?.user.PhotoName))
                        {
                            comment.user.Phpto = File.ReadAllBytes("./img/" + comment.user?.PhotoName);
                        }
                        online?.comments?.Add(comment.Com);
                        online?.users?.Add(comment.user);
                    }
                }
                response.data = new LibProtocol.Online { posts = online?.posts, comments = online?.comments, users = online?.users };
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception ex)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = $"Freand Page False: {ex}";
            }
            return response;
        }
    }
}
