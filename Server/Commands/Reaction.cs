using LibProtocol.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandServer
{
    public class Reaction
    {
        LibProtocol.Models.Reactions data;
        AddDbContext _context;
        public Reaction(LibProtocol.Models.Reactions data)
        {
            this.data = data;
            _context = new AddDbContext();
        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                var qerty = _context.Reactions.Where(r => r.IdPost == data.IdPost).Join(_context.Posts, r => r.IdPost, p => p.Id, (r, p) => new { Ract = r, Post = p }).FirstOrDefault();

                if (qerty != null)
                {
                    if (qerty.Ract.Reaction == data.Reaction && qerty.Ract.Reaction == 1)
                    {
                        qerty.Post.Like--;
                        _context.Posts.Update(qerty.Post);
                        _context.Reactions.Remove(qerty.Ract);
                    }
                    else if (qerty.Ract.Reaction == data.Reaction && qerty.Ract.Reaction == 2)
                    {
                        qerty.Post.Dislike--;
                        _context.Posts.Update(qerty.Post);
                        _context.Reactions.Remove(qerty.Ract);
                    }
                    else if (qerty.Ract.Reaction != data.Reaction && data.Reaction == 1)
                    {
                        qerty.Ract.Reaction = 1;
                        qerty.Post.Like++;
                        if (qerty.Post.Dislike != 0) qerty.Post.Dislike--;
                        _context.Posts.Update(qerty.Post);
                        _context.Reactions.Update(qerty.Ract);
                    }
                    else if (qerty.Ract.Reaction != data.Reaction && data.Reaction == 2)
                    {
                        qerty.Ract.Reaction = 2;
                        qerty.Post.Dislike++;
                        if (qerty.Post.Like != 0) qerty.Post.Like--;
                        _context.Posts.Update(qerty.Post);
                        _context.Reactions.Update(qerty.Ract);
                    }
                }
                else
                {
                    var q = _context.Posts.Where(p => p.Id == data.IdPost).FirstOrDefault();
                    if (data.Reaction == 1) q.Like++;
                    else q.Dislike++;

                    _context.Posts.Update(q);
                    _context.Reactions.Add(new Reactions { Id = Guid.NewGuid(), IdPost = data.IdPost, IdUser = data.IdUser, Reaction = data.Reaction });
                }
                _context.SaveChanges();
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Post False";
            }
            return response;
        }
    }
}
