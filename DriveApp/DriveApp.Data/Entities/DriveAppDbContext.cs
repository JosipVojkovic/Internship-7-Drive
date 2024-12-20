using DriveApp.Data.Entities.Models;
using DriveApp.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Entities
{
    public class DriveAppDbContext : DbContext
    {
        public DriveAppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Models.File> Files => Set<Models.File>();
        public DbSet<Folder> Folders => Set<Folder>();
        public DbSet<SharedItem> SharedItems => Set<SharedItem>();
        public DbSet<Comment> Comments => Set<Comment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<Models.File>("Folder")
                .HasValue<Folder>("File");

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Owner)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.OwnerId);
            
            modelBuilder.Entity<SharedItem>()
                .HasOne(si => si.Item)
                .WithMany(i => i.SharedItems)
                .HasForeignKey(si => si.ItemId);

            modelBuilder.Entity<SharedItem>()
                .HasOne(si => si.Owner)
                .WithMany(u => u.SharedItems)
                .HasForeignKey(si => si.OwnerId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Owner)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Item)
                .WithMany(i => i.Comments)
                .HasForeignKey(c => c.ItemId);

            DatabaseSeeder.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
