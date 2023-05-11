using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataContext: DbContext
    {
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Like> Likes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(user => user.FirstName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(user => user.LastName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(user => user.Email).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<User>().Property(user => user.Password).HasMaxLength(50).IsRequired();

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
  
        }
    }
}
