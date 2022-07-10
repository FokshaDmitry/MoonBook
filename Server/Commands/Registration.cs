using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Commands
{
    class Registration
    {
        LibProtocol.Models.User data;
        LibProtocol.Services.Hasher _hasher;
        AddDbContext add;

        public Registration(LibProtocol.Models.User data)
        {
            _hasher = new LibProtocol.Services.Hasher();
            this.data = data;
            add = new AddDbContext();
        }

        public bool ChekLogin()
        {
            if (add.Users.Count() != 0)
            {
                foreach (var item in add.Users)
                {
                    if (item.Login == data.Login)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public LibProtocol.Response buildResponce(ref LibProtocol.Response response)
        {
            try
            {
                String photoName = Guid.NewGuid().ToString();
                using (FileStream stream = new FileStream("./img/" + photoName + ".img", FileMode.CreateNew))
                {
                    stream.Write(data.Phpto, 0, data.Phpto.Length);
                }
                data.RegMoment = DateTime.Now;
                data.PhotoName = photoName;
                data.PassSalt = _hasher.heshString(DateTime.Now.ToString());
                data.Password = _hasher.heshString(data.Password + data.PassSalt);
                add.Users.Add(data);
                add.SaveChanges();
                response.succces = true;
                response.code = LibProtocol.ResponseCode.Ok;
                response.StatusTxt = "Add Ok";
            }
            catch (Exception)
            {
                response.succces = false;
                response.code = LibProtocol.ResponseCode.Error;
                response.StatusTxt = "Add False";
            }
            return response;
        }
    }
}
