using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoonBook
{
    /// <summary>
    /// Interaction logic for UpdateForm.xaml
    /// </summary>
    public partial class UpdateForm : Window
    {
        public Guid idUser;
        ServerConnect server;
        byte[] Photo;
        public BitmapImage imgsource;
        OpenFileDialog openFileDialog1;
        UserPageLogic page;
        public UpdateForm(Guid idUser, string Name, string Surname, byte[] Photo, string Login, string Status, DateTime Birthday, UserPageLogic page)
        {
            InitializeComponent();
            this.idUser = idUser;
            this.Name.Text = Name;
            this.Surname.Text = Surname;
            this.Date.DisplayDate = Birthday;
            this.Login.Text = Login;
            this.Status.Text = Status;
            this.Photo = Photo;
            this.page = page;
            imgsource = new BitmapImage();
            openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf",
                Title = "Open image file"
            };
            if (Photo != null)
            {
                imgsource.BeginInit();
                imgsource.StreamSource = new MemoryStream(Photo);
                imgsource.EndInit();
                UserPhoto.Fill = new ImageBrush(imgsource);
            }
            server = new ServerConnect();   
        }
        public void OpenFileDialogForm()
        {
            Dispatcher.Invoke(new Action(() => openFileDialog1.ShowDialog()));
            if (openFileDialog1.FileName != "")
            {
                try
                {
                    Dispatcher.Invoke(new Action(() => UserPhoto.Fill = new ImageBrush(new BitmapImage(new Uri(openFileDialog1.FileName)))));
                    Photo = File.ReadAllBytes(openFileDialog1.FileName);
                }
                catch (SecurityException ex)
                {
                    System.Windows.MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }
        public void Update()
        {
            if (Dispatcher.Invoke(() => Pass.Password != ""))
            {
                if (Dispatcher.Invoke(() => ConPass.Password) == "")
                {
                    System.Windows.MessageBox.Show("Enter Confirm Password");
                    return;
                }
                if (Dispatcher.Invoke(() => Pass.Password != ConPass.Password))
                {
                    System.Windows.MessageBox.Show("Password dont confirm");
                    return;
                }
            }
            server.Connect();
            Dispatcher.Invoke(() => server.UpdateData(idUser, Name.Text, Surname.Text, Status.Text, Date.DisplayDate, Login.Text, Pass.Password, Photo));
            Dispatcher.Invoke(() => 
            { 
                page.Name.Text = $"{this.Name.Text} {this.Surname.Text}";
                page.Status.Text = this.Status.Text;
                page.Photo.Fill = UserPhoto.Fill;
            });
            Dispatcher.Invoke(() => this.Close());
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Update());
        }

        private void UpdatePhoto_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => OpenFileDialogForm());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
