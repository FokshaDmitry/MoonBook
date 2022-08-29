using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Interaction logic for FreandUser.xaml
    /// </summary>
    public partial class FreandUser : Grid
    {
        public Guid UserId;
        public BitmapImage imgsource;


        public FreandUser(Guid idUser, byte[] Photo, bool Online)
        {
            InitializeComponent();
            UserId = idUser;
            imgsource = new BitmapImage();
            if (Photo != null)
            {
                imgsource.BeginInit();
                imgsource.StreamSource = new MemoryStream(Photo);
                imgsource.EndInit();
                this.Photo.Fill = new ImageBrush(imgsource);
            }
            if (Online) this.Online.Fill = System.Windows.Media.Brushes.LightGreen;
            else this.Online.Fill = System.Windows.Media.Brushes.LightGray;
        }
    }
}
