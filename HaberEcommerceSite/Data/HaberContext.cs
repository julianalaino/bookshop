using HaberEcommerceSite.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace HaberEcommerceSite.Data
{
    public class HaberContext : IdentityDbContext<User, Role, Guid>
    {
        public HaberContext(DbContextOptions<HaberContext> options) : base(options)
        {
            
        }   
           
        public DbSet<Author> Authors { get; set; }

        public DbSet<Promo> Promos { get; set; }

        public DbSet<ListDetail> ListDetail { get; set; }

        public DbSet<PriceList> PriceList { get; set; }

        public DbSet<Editorial> Editorials { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }

        public DbSet<BookCollection> BookCollections { get; set; }

        public DbSet<Bookbinding> Bookbindings { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookAuthor> BooksAuthors { get; set; }

        public DbSet<BookPromo> BooksPromos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<BookAuthor>().HasKey(t => new { t.BookId, t.AuthorId });
            modelBuilder.Entity<BookAuthor>()
           .HasOne<Book>(bookAuthor => bookAuthor.Book)
           .WithMany(book => book.BooksAuthors)
           .HasForeignKey(bookAuthor => bookAuthor.BookId);


            modelBuilder.Entity<BookAuthor>()
                .HasOne<Author>(bookAuthor => bookAuthor.Author)
                .WithMany(author => author.BooksAuthors)
                .HasForeignKey(bookAuthor => bookAuthor.AuthorId);

            modelBuilder.Entity<BookPromo>().HasKey(t => new { t.BookId, t.PromoId });
            modelBuilder.Entity<BookPromo>()
           .HasOne<Book>(bookPromo => bookPromo.Book)
           .WithMany(book => book.BooksPromos)
           .HasForeignKey(bookPromo => bookPromo.BookId);


            modelBuilder.Entity<BookPromo>()
                .HasOne<Promo>(bookPromo => bookPromo.Promo)
                .WithMany(promo => promo.BooksPromos)
                .HasForeignKey(bookPromo => bookPromo.PromoId);

            

            modelBuilder.Entity<IdentityUser<Guid>>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(property => property.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.ToTable("Roles");

                entity.Property(property => property.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("UserRoles");

                entity.Property(property => property.RoleId).HasColumnName("ID");
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims");

                entity.Property(property => property.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins");

                entity.Property(property => property.UserId).HasColumnName("ID");
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens");

                entity.Property(property => property.UserId).HasColumnName("ID");
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaims");

                entity.Property(property => property.Id).HasColumnName("ID");
            });

        }
    }
}
