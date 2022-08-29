using EpubSharp;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MoonBook
{
    public class UserPageLogic : UserPageView
    {

        public UserPageLogic(Guid guid, string? name, string? surname, string status, byte[] photo, DateTime birthday, string login, UserPage main) : base(guid, name, surname, status, photo, birthday, login, main)
        {

        }
        public override void OpenFileDialogForm()
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                try
                {
                    Dispatcher.Invoke(() => CheckImg.Fill = new ImageBrush(new BitmapImage(new Uri(openFileDialog1.FileName))));
                    Dispatcher.Invoke(() => Path.Text = openFileDialog1.FileName);
                    Dispatcher.Invoke(() => ImageByte = File.ReadAllBytes(openFileDialog1.FileName));
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }
        public void Check()
        {
            while (tokenstop)
            {
                int num = 0;
                int tmp = 0;
                server.Connect();
                server.chekOnline(idUser);
                server.waitResponse((res) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        foreach (Post item in VewPost.Items)
                        {
                            num += item.ListComments.Items.Count;
                            num += Convert.ToInt32(item.Like.Text);
                            num += Convert.ToInt32(item.Dislike.Text);
                        }
                    });
                    num += VewPost.Items.Count;
                    num += ListBookUser.Items.Count;
                    tmp = (int)res.data;

                });
                if (tmp != num)
                    Task.Run(() => Online());
                Thread.Sleep(1000);
            }
        }
        
        public void Online()
        {
            server.Connect();
            server.monOnline(idUser);
            server.waitResponse((res) => onlineLib = (LibProtocol.Online)res.data);
            Dispatcher.Invoke(() => 
            {
                VewPost.Items.Clear();
                ListBookUser.Items.Clear();
                FreandList.Items.Clear();
                foreach (var post in onlineLib?.posts.Join(onlineLib.users, p => p.IdUser, u => u.Id, (p, u) => new { pos = p, use = u }).Distinct())
                {
                    VewPost.Items.Add(new Post($"{post.use.Name} {post.use.Surname}", post.use.Phpto, post.pos.Text, post.pos.Title, post.pos.Image, post.pos.Date, post.pos.Like, post.pos.Dislike, post.pos.Id, idUser, onlineLib));
                }
                foreach (var book in onlineLib.books)
                {
                    ListBookUser.Items.Add(new Book(book.Id, book.CoverImage, book.Title, book.Author, book.TextContent, book.idUser));
                }
                foreach (var users in onlineLib.subscriptions.Where(s => s.IdUser == idUser).Join(onlineLib.users, s => s.IdFreand, u => u.Id, (s, u) => new { Sub = s, User = u }))
                {
                    FreandList.Items.Add(new FreandUser(users.User.Id, users.User.Phpto, users.User.Online));
                }
            });
        }
        
        public override void NewPost()
        {
            NewText.Text.Replace($">{title.ToUpper()}<", "");
            server.Connect();
            server.addPost(idUser, Name.Text, title, NewText.Text, "");
            NewText.Text = "";
            Path.Text = "";
            title = "";
            CheckImg.Fill = null;
        }
        public override void addBook()
        {
            AddBookForm addBookForm;
            Dispatcher.Invoke(() =>
            {
                addBookForm = new AddBookForm(idUser);
                //addBookForm.Owner = main;
                addBookForm.Show();
            });
        }

        //public override void RemoveLibrary()
        //{
        //    server.Connect();
        //    Dispatcher.Invoke(() => server.removeLibrary(idUser, ReadNow));
        //}
        //public override void AddLibrary()
        //{
        //    server.Connect();
        //    Dispatcher.Invoke(() => server.addLibrary(idUser, ReadNow));
        //}
    }
}
