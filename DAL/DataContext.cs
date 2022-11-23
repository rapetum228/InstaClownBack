using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: отдельный метод в отдельном классе
            modelBuilder
                .Entity<User>()
                .HasIndex(f => f.Email)
                .IsUnique(); //уникальность email
            modelBuilder
               .Entity<User>()
               .HasIndex(f => f.Name)
               .IsUnique();
            modelBuilder.Entity<Avatar>().ToTable(nameof(Avatars));
            modelBuilder.Entity<PostAttach>().ToTable(nameof(PostAttaches));
            modelBuilder.Entity<MessageAttach>().ToTable(nameof(MessageAttaches));

            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(p => p.Name).IsUnique();
           

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(b => b.MigrationsAssembly("Api"));

        public DbSet<User> Users => Set<User>();
        public DbSet<UserSession> UserSessions => Set<UserSession>();
        public DbSet<Attach> Attaches => Set<Attach>();
        public DbSet<Avatar> Avatars => Set<Avatar>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<PostAttach> PostAttaches => Set<PostAttach>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<MessageAttach> MessageAttaches => Set<MessageAttach>();
    }
}
