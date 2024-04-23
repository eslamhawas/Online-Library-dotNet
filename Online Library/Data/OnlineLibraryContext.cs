using Microsoft.EntityFrameworkCore;
using Online_Library.Models;

namespace Online_Library.Data
{
    public partial class OnlineLibraryContext : DbContext
    {
        public OnlineLibraryContext()
        {
        }

        public OnlineLibraryContext(DbContextOptions<OnlineLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Isbn)
                    .HasName("PK__Books__447D36EB41D29538");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RackNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BorrowedBook>(entity =>
            {


                entity.Property(e => e.BookIsbn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BookISBN");

                entity.Property(e => e.DateOfReturn).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.BookIsbnNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.BookIsbn)
                    .HasConstraintName("FK__BorrowedB__BookI__45F365D3");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__BorrowedB__UserI__46E78A0C");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PassordSalt).HasMaxLength(1000);

                entity.Property(e => e.PasswordHash).HasMaxLength(1000);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
