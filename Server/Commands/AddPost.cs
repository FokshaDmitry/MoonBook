﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandServer
{
    public class AddPost
    {
        LibProtocol.Models.Posts data;
        AddDbContext add;
        public AddPost(LibProtocol.Models.Posts data)
        {
            this.data = data;
            add = new AddDbContext();

        }

        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                String photoName = Guid.NewGuid().ToString();
                using (FileStream stream = new FileStream("./img_post/" + photoName + ".png", FileMode.CreateNew))
                {
                    stream.Write(data.ImageMass, 0, data.ImageMass.Length);
                }
                data.Image = photoName + ".png";
                add.Posts.Add(data);
                add.SaveChanges();
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
