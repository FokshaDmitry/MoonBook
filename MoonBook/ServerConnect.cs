using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoonBook
{
    public class ServerConnect
    {
        public IPAddress ip = IPAddress.Parse("127.0.0.1");
        public int port = 3456;

        public event Action<string>? onError;
        TcpClient tpcClient;
        Thread threadClient;
        NetworkStream stream;
        UdpClient udpClient;

        BinaryFormatter bf = new BinaryFormatter();

        public void Connect()
        {
            try
            {
                tpcClient = new TcpClient();
                tpcClient.Connect(new IPEndPoint(ip, port));
                stream = tpcClient.GetStream();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void newAccaunt(string name, string surname, DateTime date, string email, string login, string password, byte[] photo)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Registration;
            request.data = new LibProtocol.Models.User { Id = Guid.NewGuid(), Name = name, Email = email, Surname = surname, DateOfBith = date, Login = login, Password = password, Phpto = photo };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void addPost(Guid idU, string title, string text, byte[] img)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.addPost;
            request.data = new LibProtocol.Models.Posts { Id = Guid.NewGuid(), Date = DateTime.Now, Title = title, Text = text, ImageMass = img, Like = 0, Dislike = 0, IdUser = idU };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void addComent(DateTime date, string text, Guid idpost, Guid iduser)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Comment;
            request.data = new LibProtocol.Models.Comments { Id = Guid.NewGuid(), Date = date, Text = text, idPost = idpost, idUser = iduser };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void LoginAccaunt(string log, string pass)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Login;
            request.data = new LibProtocol.Models.User { Login = log, Password = pass };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void UpdateData(Guid idUser, string name, string surname, string status, DateTime date, string login, string password, byte[] photo)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Update;
            request.data = new LibProtocol.Models.User { Id = idUser, Name = name, Surname = surname, Status = status, DateOfBith = date, Login = login, Password = password, Phpto = photo};

            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void Exit(Guid idUser)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Exit;
            request.data = idUser;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void addReaction(int reaction, Guid idpost, Guid iduser)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Reaction;
            request.data = new LibProtocol.Models.Reactions { Id = Guid.NewGuid(), IdUser = iduser, Reaction = reaction, IdPost = idpost };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void Subscribe(Guid iduser, Guid idfreand)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Subscription;
            request.data = new LibProtocol.Models.Subscriptions { Id = Guid.NewGuid(), IdUser = iduser, IdFreand = idfreand };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void FreandPage(Guid idFrend)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.FreandPage;
            request.data = idFrend;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void addLineFreand(Guid idUser)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.LineBlog;
            request.data = idUser;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void UnSubscribe(Guid iduser, Guid idfreand)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.UnSubscription;
            request.data = new LibProtocol.Models.Subscriptions { IdUser = iduser, IdFreand = idfreand };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void addLibrary(Guid iduser, Guid idbook)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.addLibrary;
            request.data = new LibProtocol.Models.SubBook { Id = Guid.NewGuid(), idUser = iduser, idBook = idbook };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void removeLibrary(Guid iduser, Guid idbook)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.removeLibrary;
            request.data = new LibProtocol.Models.SubBook { idUser = iduser, idBook = idbook };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void Read(Guid iduser, Guid idbook)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.ChekBook;
            request.data = new LibProtocol.Models.SubBook { idUser = iduser, idBook = idbook };
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void DeleteBook(Guid id)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.DeleteBook;
            request.data = id;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void DeleteComment(Guid id)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.DeleteComment;
            request.data = id;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void Delete(Guid id)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Remove;
            request.data = id;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void chekOnline(Guid id)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Chek;
            request.data = id;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void OnlineFreands(Guid idUser)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.OnlineFreands;
            request.data = idUser;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void Search(LibProtocol.Models.User user)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Search;
            request.data = user;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void SearchBook(string searchtext)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.SearchBook;
            request.data = searchtext;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void monOnline(Guid iduser)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.Online;
            request.data = iduser;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void OnlineBook(Guid iduser)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.OnlineBook;
            request.data = iduser;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void addBook(Guid IdUser, byte[] coverImage, string author, string title, String toPlainText)
        {
            LibProtocol.Request request = new LibProtocol.Request();
            request.command = LibProtocol.Command.addBook;
            request.data = new LibProtocol.Models.Books{ Id = Guid.NewGuid(), Author = author, CoverImage = coverImage,  Title = title, TextContent = toPlainText, idUser = IdUser};
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void waitResponse(Action<LibProtocol.Response> onOk)
        {
            LibProtocol.Response response = (LibProtocol.Response)bf.Deserialize(stream);
            if (response.succces)
            {
                onOk(response);
            }
            else
            {
                onError?.Invoke(response.StatusTxt);
            }
        }
    }
}
