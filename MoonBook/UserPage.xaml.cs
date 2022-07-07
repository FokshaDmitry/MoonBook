using EpubSharp;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
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
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private int selector;
        public ServerConnect server;
        public Guid idUser;
        public MainWindow main;
        public BitmapImage imgsource;
        public UserPageLogic logic;
        public BookPageLogic booklogic;
        public BlogPage blog;
        public FreandPageLogic freand;
        public UserPage(Guid guid, string? name, string? surname, string status, byte[] photo, DateTime birthday, bool online, string login, MainWindow main)
        {
            InitializeComponent();
            selector = 0;
            idUser = guid;
            logic = new UserPageLogic(idUser, name, surname, status, photo, birthday, login, this);
            booklogic = new BookPageLogic(idUser);
            blog = new BlogPage(idUser);
            freand = new FreandPageLogic(idUser, this);

            imgsource = new BitmapImage();
            if (photo != null)
            {
                imgsource.BeginInit();
                imgsource.StreamSource = new MemoryStream(photo);
                imgsource.EndInit();
                minPhoto.Fill = new ImageBrush(imgsource);
            }
            if (online) OnlineStatus.Fill = System.Windows.Media.Brushes.LightGreen;
            else OnlineStatus.Fill = System.Windows.Media.Brushes.LightGray;
            server = new ServerConnect();
            server.onError += mess => MessageBox.Show(mess);
        }

        public void Update()
        {
            UpdateForm update;
            Dispatcher.Invoke(() =>
            {
                update = new UpdateForm(idUser, logic.name, logic.surname, logic.photo, logic.login, logic.status, logic.birthday, logic);
                update.Owner = main;
                update.Show();
            });

        }
        public void Exit()
        {
            server.Connect();
            server.Exit(idUser);
            main.Close();
        }

        private void Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Control.SelectedIndex == 0)
            {
                if (selector != 1)
                {
                    logic.tokenstop = true;
                    Task.Run(() => logic.Check());
                    Page1.Content = logic;
                    selector = 1;
                }
            }
            if (Control.SelectedIndex == 1)
            {
                if (selector != 2)
                {
                    logic.tokenstop = false;
                    Page2.Content = booklogic;
                    Task.Run(() => booklogic.Book());
                    selector = 2;
                }
            }
            if (Control.SelectedIndex == 2)
            {
                if (selector != 3)
                {
                    logic.tokenstop = false;
                    Page3.Content = blog;
                    Task.Run(() => blog.LineBlog());
                    selector = 3;
                }
            }
            if (Control.SelectedIndex == 3)
            {
                if (selector != 4)
                {
                    logic.tokenstop = false;
                    Page4.Content = freand;
                    Task.Run(() => freand.OnlineFreands());
                    selector = 4;
                }
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Exit());
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Update());
        }
    }
}
