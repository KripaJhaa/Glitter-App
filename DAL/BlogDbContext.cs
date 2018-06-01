using ModelsDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext() : base("BlogDbContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<UserLink> UserLinks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
