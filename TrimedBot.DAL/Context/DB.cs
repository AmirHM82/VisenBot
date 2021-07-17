using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrimedBot.DAL.Entities;

namespace TrimedBot.DAL.Context
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TempMessage> TempMessages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Banner> Banners { get; set; }
    }
}
