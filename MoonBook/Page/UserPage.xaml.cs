using Microsoft.Win32;
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
    /// Interaction logic for UserPageView.xaml
    /// </summary>

    public abstract partial class UserPage : Grid 
    {
        public string title;
        public Guid ReadNow;
        public bool tokenstop;
        public ServerConnect server;
        public LibProtocol.Online onlineLib;
        public Guid idUser;
        public string login;
        public MoonBookPage main;
        public string name;
        public string surname;
        public string status;
        public byte[] photo;
        public DateTime birthday;
        public OpenFileDialog openFileDialogEpub;
        public byte[]? ImageByte;
        public OpenFileDialog openFileDialog1;
        public BitmapImage imgsource;
        public UserPage(Guid guid, string? name, string? surname, string status, byte[] photo, DateTime birthday, string login, MoonBookPage main)
        {
            InitializeComponent();
            title = "";
            ReadNow = new Guid();
            idUser = guid;
            tokenstop = false;
            this.main = main;
            this.login = login;
            this.name = name;
            this.surname = surname;
            this.photo = photo;
            this.status = status;
            this.birthday = birthday;
            Name.Text = $"{name} {surname}";
            Status.Text = status;
            title = "";
            imgsource = new BitmapImage();
            if (photo != null)
            {
                imgsource.BeginInit();
                imgsource.StreamSource = new MemoryStream(photo);
                imgsource.EndInit();
                Photo.Fill = new ImageBrush(imgsource);
            }
            openFileDialogEpub = new OpenFileDialog()
            {
                Filter = "Image files (*.EPUB)|*.epub",
                Title = "Open epub file"
            };
            onlineLib = new LibProtocol.Online();
            server = new ServerConnect();
            server.onError += mess => MessageBox.Show(mess);
        }
        public virtual void OpenFileDialogForm() { }
        public virtual void addBook() { }
        public virtual void NewPost() { }
        public virtual void Online() { }
        //public virtual void AddLibrary();
        //public virtual void RemoveLibrary();
        private void Title_Click(object sender, RoutedEventArgs e)
        {
            title = NewText.SelectedText;
            NewText.SelectedText = $">{NewText.SelectedText.ToUpper()}<";
        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            NewPost();
        }
        private void Image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialogForm();
        }
        private void FreandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FreandUser user = (FreandUser)FreandList.SelectedItem;
            Task.Run(() => main.freand.FrendPage(user.UserId));
            main.Control.SelectedIndex = 3;

        }
        private void ListBookUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book book = (Book)ListBookUser.SelectedItem;
            Task.Run(() => main.booklogic.ReadBook(book.Id, book.Text));
            main.Control.SelectedIndex = 1;
        }
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => addBook());
        }
    }
}
