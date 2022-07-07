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
using System.IO;

namespace MoonBook
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Grid
    {
        public Guid IdUser;
        public Guid IdFreand;
        BitmapImage imgsource;
        ServerConnect server;
        public bool freand;
        FreandPageLogic page;
        public User(Guid IdUser, string Name, byte[] Photo, bool Online, bool Follow, Guid IdFreand, FreandPageLogic page)
        {
            InitializeComponent();
            this.IdUser = IdUser;
            this.IdFreand = IdFreand;
            this.Name.Text = Name;
            imgsource = new BitmapImage();
            freand = Follow;
            this.page = page;
            if (Photo != null)
            {
                imgsource.BeginInit();
                imgsource.StreamSource = new MemoryStream(Photo);
                imgsource.EndInit();
                this.Photo.Fill = new ImageBrush(imgsource);
            }
            if (Online) this.Online.Fill = System.Windows.Media.Brushes.LightGreen;
            else this.Online.Fill = System.Windows.Media.Brushes.LightGray;
            if (Follow) this.Follow.Background = new ImageBrush(new BitmapImage(new Uri("person_remove_FILL0_wght400_GRAD0_opsz48.png", UriKind.RelativeOrAbsolute)));
            else this.Follow.Background = new ImageBrush(new BitmapImage(new Uri("person_add_FILL0_wght400_GRAD0_opsz48.png", UriKind.RelativeOrAbsolute)));
            server = new ServerConnect();
        }

        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            server.Connect();
            if (freand)
            {
                server.UnSubscribe(IdUser, IdFreand);
                this.Follow.Background = new ImageBrush(new BitmapImage(new Uri("person_remove_FILL0_wght400_GRAD0_opsz48.png", UriKind.RelativeOrAbsolute)));
                page.OnlineFreands();
            }
            else
            {
                server.Subscribe(IdUser, IdFreand);
                this.Follow.Background = new ImageBrush(new BitmapImage(new Uri("person_add_FILL0_wght400_GRAD0_opsz48.png", UriKind.RelativeOrAbsolute)));
                page.OnlineFreands();
            }

        }
    }
}
