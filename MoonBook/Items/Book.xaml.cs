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
    /// Логика взаимодействия для Book.xaml
    /// </summary>
    public partial class Book : Grid
    {
        public Guid Id;
        public Guid? IdUser;
        public String Text;
        public byte[] Cover;
        public string Title;
        public string Author;
        public Book(Guid id , byte[] Image, string Title, string Author, String Text, Guid? idUser)
        {
            InitializeComponent();
            if (Image != null)
            {
                BitmapImage imgs = new BitmapImage();
                imgs.BeginInit();
                imgs.StreamSource = new MemoryStream(Image);
                imgs.EndInit();
                this.Imege.Fill = new ImageBrush(imgs);
            }
            this.Author = Author;
            Cover = Image;
            Id = id;
            IdUser = idUser;
            this.Text = Text;
            this.Title = Title;
        }
    }
}