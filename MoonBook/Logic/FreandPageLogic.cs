using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MoonBook.Logic
{
    public class FreandPageLogic : FreandPage
    {
        public FreandPageLogic(Guid idUser, UserPage main) : base(idUser, main)
        {

        }

        public override void OnlineFreands()
        {
            string tmp = null;
            Dispatcher.Invoke(() => tmp = SeachText.Text);
            server.Connect();
            if (tmp == "")
            {
                server.OnlineFreands(idUser);
                server.waitResponse((res) =>
                {
                    onlineLib = (LibProtocol.Online)res.data;
                    Dispatcher.Invoke(() =>
                    {
                        ListUser.Items.Clear();
                        foreach (var user in onlineLib.users.Where(u => u.Online == true))
                        {
                            ListUser.Items.Add(new User(idUser, $"{user.Name} {user.Surname}", user.Phpto, user.Online, true, user.Id, this));
                        }
                        foreach (var user in onlineLib.users.Where(u => u.Online == false))
                        {
                            ListUser.Items.Add(new User(idUser, $"{user.Name} {user.Surname}", user.Phpto, user.Online, true, user.Id, this));
                        }
                    });
                });
            }
            else
            {
                Dispatcher.Invoke(() => server.Search(new LibProtocol.Models.User { Id = idUser, Name = SeachText.Text.Replace(" ", "") }));
                server.waitResponse((res) =>
                {
                    onlineLib = (LibProtocol.Online)res.data;
                    Dispatcher.Invoke(() =>
                    {
                        ListUser.Items.Clear();
                        foreach (var user in onlineLib.subscriptions.Join(onlineLib.users, s => s.IdFreand, u => u.Id, (s, u) => new { Sub = s, Use = u }))
                        {
                            ListUser.Items.Add(new User(idUser, $"{user.Use.Name} {user.Use.Surname}", user.Use.Phpto, user.Use.Online, true, user.Use.Id, this));
                            onlineLib.users.Remove(user.Use);
                        }
                        foreach (var user in onlineLib.users)
                        {
                            ListUser.Items.Add(new User(idUser, $"{user.Name} {user.Surname}", user.Phpto, user.Online, false, user.Id, this));

                        }
                    });

                });
            }
        }

        public override void FrendPage(Guid idFreand)
        {
            server.Connect();
            server.FreandPage(idFreand);
            server.waitResponse((res) =>
            {
                onlineLib = (LibProtocol.Online)res.data;
                Dispatcher.Invoke(() =>
                {
                    NameFreand.Text = $"{onlineLib.users.Where(u => u.Id == idFreand).Select(u => u.Name).First()} {onlineLib.users.Where(u => u.Id == idFreand).Select(u => u.Surname).First()}";
                    StatusFreands.Text = $"{onlineLib.users.Where(u => u.Id == idFreand).Select(u => u.Status).First()}";
                    byte[] pho = onlineLib.users.Where(u => u.Id == idFreand).Select(u => u.Phpto).First();
                    if (pho != null)
                    {
                        BitmapImage imgs = new BitmapImage();
                        imgs.BeginInit();
                        imgs.StreamSource = new MemoryStream(pho);
                        imgs.EndInit();
                        PhotoFreand.Fill = new ImageBrush(imgs);
                    }
                    BlogListFreand.Items.Clear();
                    FreandBook.Items.Clear();
                    FrendsFreadList.Items.Clear();
                    foreach (var post in onlineLib.posts.Join(onlineLib.users, p => p.IdUser, u => u.Id, (p, u) => new { pos = p, use = u }).Distinct())
                    {
                        BlogListFreand.Items.Add(new Post(post.use.Name + " " + post.use.Surname, post.use.Phpto, post.pos.Text, post.pos.Title, post.pos.Image, post.pos.Date, post.pos.Like, post.pos.Dislike, post.pos.Id, idUser, onlineLib));
                    }
                    foreach (var book in onlineLib.books)
                    {
                        FreandBook.Items.Add(new Book(book.Id, book.CoverImage, book.Title, book.Author, book.TextContent, book.idUser));
                    }
                    foreach (var sub in onlineLib.subscriptions.Join(onlineLib.users, s => s.IdFreand, u => u.Id, (s, u) => new { Sub = s, User = u }))
                    {
                        FrendsFreadList.Items.Add(new FreandUser(sub.User.Id, sub.User.Phpto, sub.User.Online));
                    }
                });

            });
        }
    }
}
