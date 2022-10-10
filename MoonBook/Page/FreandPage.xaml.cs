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
    /// <summary>
    /// Interaction logic for FreandPage.xaml
    /// </summary>
    public partial class FreandPage : Grid
    {
        public MoonBookPage main;
        public Guid idUser;
        public ServerConnect server;
        public LibProtocol.Online onlineLib;
        public FreandPage(Guid idUser , MoonBookPage main)
        {
            InitializeComponent();
            this.main = main;
            this.idUser = idUser;
            server = new ServerConnect();
            server.onError += mess => MessageBox.Show(mess);
            onlineLib = new LibProtocol.Online();   
        }
        public virtual void OnlineFreands() { }
        public virtual void FrendPage(Guid id) { }

        private void FreandBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book book = (Book)FreandBook.SelectedItem;
            Task.Run(() => main.booklogic.ReadBook(book.Id, book.Text));
            //ReadNow = book.Id;
            main.Control.SelectedIndex = 1;

        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => OnlineFreands());
        }
        private void ListUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = (User)ListUser.SelectedItem;
            if (user != null);
            Task.Run(() => FrendPage(user.IdFreand));
        }

        private void FrendsFreadList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FreandUser user = (FreandUser)FrendsFreadList.SelectedItem;
            if (user != null);
            Task.Run(() => FrendPage(user.UserId));
        }
    }
}
