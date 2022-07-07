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
    public partial class BookPage : Grid
    {
        public ServerConnect server;
        public LibProtocol.Online onlineLib;
        public Guid ReadNow;
        public Guid idUser;
        public BookPage(Guid idUser)
        {
            InitializeComponent();
            this.idUser = idUser;
            ReadNow = new Guid(); 
            server = new ServerConnect();
            onlineLib = new LibProtocol.Online();
            server.onError += mess => MessageBox.Show(mess);
        }
        public virtual void ReadBook(Guid id, String text) { }
        public virtual void Book() { }
        private void AddBookLibrary_Click(object sender, RoutedEventArgs e)
        {
            //добавить вариант удоление removeLibrary
            //Task.Run(() => AddLibrary());
        }
        private void SearchBook_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Book());
        }
        private void Library_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookItem bookItem = (BookItem)Library.SelectedItem;
            if(bookItem != null)
            {
                Task.Run(() => ReadBook(bookItem.Id, bookItem.Text));
                ReadNow = bookItem.Id;
            }
        }
        private void MyListBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookItem bookItem = (BookItem)MyListBook.SelectedItem;
            if(bookItem != null)
            {
                Task.Run(() => ReadBook(bookItem.Id, bookItem.Text));
                ReadNow = bookItem.Id;
            }
        }
        private void White_cck(object sender, RoutedEventArgs e)
        {
            p1.Foreground = System.Windows.Media.Brushes.White;
        }

        private void Black_Click(object sender, RoutedEventArgs e)
        {
            p1.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            p1.FontStyle = FontStyles.Oblique;
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            p1.FontStyle = FontStyles.Italic;
        }

        private void TextLeft_Click(object sender, RoutedEventArgs e)
        {
            p1.TextAlignment = TextAlignment.Left;
        }

        private void TextRight_click(object sender, RoutedEventArgs e)
        {
            p1.TextAlignment = TextAlignment.Right;
        }

        private void TextLustify_Click(object sender, RoutedEventArgs e)
        {
            p1.TextAlignment = TextAlignment.Justify;
        }

        private void TextCenter_Click(object sender, RoutedEventArgs e)
        {
            p1.TextAlignment = TextAlignment.Center;
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            p1.FontStyle = FontStyles.Normal;
        }

        private void ComboText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            p1.FontFamily = new System.Windows.Media.FontFamily((string)ComboText.SelectedItem);
        }

        private void Interval_Click(object sender, RoutedEventArgs e)
        {
            p1.LineHeight = 15;
        }

        private void Interval_Click1(object sender, RoutedEventArgs e)
        {
            p1.LineHeight = 25;
        }
        private void Interval_Click2(object sender, RoutedEventArgs e)
        {
            p1.LineHeight = 35;
        }
    }
}
