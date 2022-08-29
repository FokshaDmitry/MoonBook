using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MoonBook.Logic
{
    public class BookPageLogic : BookPage
    {
        public BookPageLogic(Guid idUser) : base(idUser)
        {
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                ComboText.Items.Add(fontFamily.Source);
            }
        }
        public override void ReadBook(Guid idbook, string text)
        {
            server.Connect();
            server.Read(idUser, idbook);
            server.waitResponse((res) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if ((bool)res.data)
                    {
                        AddBookLibrary.Background = new ImageBrush(new BitmapImage(new Uri("close_FILL0_wght400_GRAD0_opsz48.png", UriKind.RelativeOrAbsolute)));
                    }
                    else
                    {
                        AddBookLibrary.Background = new ImageBrush(new BitmapImage(new Uri("done_FILL0_wght400_GRAD0_opsz48.png", UriKind.RelativeOrAbsolute)));
                    }
                    Task.Run(() => Book());
                    p1.Inlines.Clear();
                    p1.Inlines.Add(text);
                });

            });

        }
        public override void Book()
        {
            string tmp = "";
            Dispatcher.Invoke(() => tmp = SeachBook.Text);
            server.Connect();
            if (string.IsNullOrEmpty(tmp))
            {
                server.OnlineBook(idUser);
                server.waitResponse((res) => onlineLib = (LibProtocol.Online)res.data);
                Dispatcher.Invoke(() =>
                {
                    Library.Items.Clear();
                    MyListBook.Items.Clear();
                    foreach (var book in onlineLib?.books.Where(b => b.idUser == idUser))
                    {
                        MyListBook.Items.Add(new BookItem(book.Id, book.CoverImage, book.Title, book.Author, book.TextContent, book.idUser));
                    }
                    foreach (var lib in onlineLib.subBook.Join(onlineLib.books, s => s.idBook, b => b.Id, (s, b) => new { sub = s, book = b }))
                    {
                        Library.Items.Add(new BookItem(lib.book.Id, lib.book.CoverImage, lib.book.Title, lib.book.Author, lib.book.TextContent, lib.book.idUser));
                    }
                });
            }
            else if (!string.IsNullOrEmpty(tmp))
            {
                server.SearchBook(tmp.Replace(" ", ""));
                server.waitResponse((res) => onlineLib = (LibProtocol.Online)res.data);
                Dispatcher.Invoke(() =>
                {
                    Library.Items.Clear();
                    foreach (var lib in onlineLib.subBook.Join(onlineLib.books, s => s.idBook, b => b.Id, (s, b) => new { sub = s, book = b }))
                    {
                        Library.Items.Add(new BookItem(lib.book.Id, lib.book.CoverImage, lib.book.Title, lib.book.Author, lib.book.TextContent, lib.book.idUser));
                    }
                });
            }
        }
    }
}
