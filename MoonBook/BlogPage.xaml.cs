using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoonBook
{
    public partial class BlogPage : Grid
    {
        public Guid idUser;
        ServerConnect server;
        public LibProtocol.Online onlineLib;
        public BlogPage(Guid idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            server = new ServerConnect(); 
            onlineLib = new LibProtocol.Online();
            server.onError += mess => MessageBox.Show(mess);
        }
        public void LineBlog()
        {
            server.Connect();
            server.addLineFreand(idUser);
            server.waitResponse((res) => onlineLib = (LibProtocol.Online)res.data);
            Dispatcher.Invoke(() =>
            {
                ListLineBlog.Items.Clear();
                foreach (var post in onlineLib.posts.Join(onlineLib.users, p => p.IdUser, u => u.Id, (p, u) => new { pos = p, use = u }).Distinct())
                {
                    ListLineBlog.Items.Add(new Post($"{post.use.Name} {post.use.Surname}", post.use.Phpto, post.pos.Text, post.pos.Title, post.pos.Image, post.pos.Date, post.pos.Like, post.pos.Dislike, post.pos.Id, idUser, onlineLib));
                }
            });
        }
    }
}
