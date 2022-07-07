using EpubSharp;
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
    /// Interaction logic for AddBookForm.xaml
    /// </summary>
    public partial class AddBookForm : Window
    {
        ServerConnect server;
        public Guid idUser;
        private OpenFileDialog openFileDialog1;
        private OpenFileDialog openFileDialogEpub;
        public EpubBook book;
        public byte[] CoverByte; 
        public AddBookForm(Guid idUser)
        {
            InitializeComponent();
            server = new ServerConnect();
            this.idUser = idUser;
            openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf",
                Title = "Open image file"
            };
            openFileDialogEpub = new OpenFileDialog()
            {
                Filter = "Image files (*.EPUB)|*.epub",
                Title = "Open epub file"
            };
            book = new EpubBook();
        }
        public void OpenFileDialogForm()
        {
            Dispatcher.Invoke(() => openFileDialog1.ShowDialog());
            if (openFileDialog1.FileName != "")
            {
                try
                {
                    Dispatcher.Invoke(() => this.Cover.Background = new ImageBrush(new BitmapImage(new Uri(openFileDialog1.FileName))));
                    Dispatcher.Invoke(() => CoverByte = File.ReadAllBytes(openFileDialog1.FileName));
                }
                catch (SecurityException ex)
                {
                    System.Windows.MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }
        public void OpenFileDialogFormEpub()
        {
            Dispatcher.Invoke(() => openFileDialogEpub.ShowDialog());
            if (openFileDialogEpub.FileName != "")
            {
                try
                {
                   Dispatcher.Invoke(() => book = EpubReader.Read(openFileDialogEpub.FileName));

                }
                catch (SecurityException ex)
                {
                    System.Windows.MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
            Dispatcher.Invoke(() => 
            {
                Title.Text = book.Title;
                Author.Text = book.Authors.Single();
                Text.Text = book.ToPlainText();
                if (book.CoverImage != null)
                {
                    BitmapImage imgsource = new BitmapImage();
                    imgsource.BeginInit();
                    imgsource.StreamSource = new MemoryStream(book.CoverImage);
                    imgsource.EndInit();
                    Cover.Background = new ImageBrush(imgsource);
                    CoverByte = book.CoverImage;

                }
            });
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            server.Connect();
            Dispatcher.Invoke(() => server.addBook(idUser, CoverByte, Author.Text, Title.Text, $"{Genre.SelectedItem.ToString()}\n {Author.Text}\n {Title.Text}\n {Anatation.Text}\n\n {Text.Text}"));
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Task.Run(() => OpenFileDialogForm());
        }

        private void AddEpub_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => OpenFileDialogFormEpub());
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}