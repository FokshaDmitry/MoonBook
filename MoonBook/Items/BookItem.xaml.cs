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
    public partial class BookItem : Grid
    {
        public string Title;
        public string Text;
        ServerConnect server;
        public Guid Id;
        public BookItem(Guid id, byte[] Image, string Title, string Author, string Text, Guid? idUser)
        {
            InitializeComponent();
            server = new ServerConnect();
            if (Image != null)
            {
                BitmapImage imgs = new BitmapImage();
                imgs.BeginInit();
                imgs.StreamSource = new MemoryStream(Image);
                imgs.EndInit();
                this.Cover.Fill = new ImageBrush(imgs);
            }
            Id = id;
            this.Name.Text = Title;
            this.Title = Title;
            this.Text = Text;
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            server.Connect();
            server.DeleteBook(Id);
        }
    }
}
