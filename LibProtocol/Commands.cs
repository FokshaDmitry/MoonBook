using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProtocol
{
    [Serializable]
    public enum Command
    {
        Registration, Login, addPost, Online, Reaction, Comment, OnlineComment, Chek, Remove, Subscription, UnSubscription, Search, OnlineFreands, FreandPage, LineBlog, addBook, OnlineBook, DeleteComment, DeleteBook, Exit, Update, ChekBook, addLibrary, removeLibrary, SearchBook
    }
}
