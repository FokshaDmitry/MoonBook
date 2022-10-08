using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class AddDbContext : DbContext
    {

        public DbSet<LibProtocol.Models.User> Users { get; set; }
        public DbSet<LibProtocol.Models.Posts> Posts { get; set; }
        public DbSet<LibProtocol.Models.Comments> Comments { get; set; }
        public DbSet<LibProtocol.Models.Reactions> Reactions { get; set; }
        public DbSet<LibProtocol.Models.Subscriptions> Subscriptions { get; set; }
        public DbSet<LibProtocol.Models.Books> Books { get; set; }
        public DbSet<LibProtocol.Models.SubBook> SubBooks { get; set; }
        public DbSet<LibProtocol.Models.DeleteList> DeleteLists { get; set; }
        public AddDbContext()
        { 
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Проекты\MoonBookWeb\MoonBookWeb\DbaseMB.mdf;Integrated Security=True");
        }
    }
}
