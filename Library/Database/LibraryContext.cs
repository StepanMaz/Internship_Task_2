using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

using Database.Entities;

namespace Database
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Rating { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 1, Title = "Book 1 Title", Cover = "111",  Content = "lSome content for book 1", Author = "Author1", Genere = "Comedy"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 2, Title = "Book 2 Title", Cover = "222",  Content = "kSome content for book 2", Author = "Author1", Genere = "Comedy"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 3, Title = "Book 3 Title", Cover = "333",  Content = "jSome content for book 3", Author = "Author2", Genere = "Comedy"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 4, Title = "Book 4 Title", Cover = "444",  Content = "hSome content for book 4", Author = "Author2", Genere = "Horror"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 5, Title = "Book 5 Title", Cover = "555",  Content = "fSome content for book 5", Author = "Author3", Genere = "Horror"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 6, Title = "Book 6 Title", Cover = "666",  Content = "eSome content for book 6", Author = "Author3", Genere = "Horror"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 7, Title = "Book 7 Title", Cover = "777",  Content = "dSome content for book 7", Author = "Author4", Genere = "Horror"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 8, Title = "Book 8 Title", Cover = "888",  Content = "cSome content for book 8", Author = "Author4", Genere = "Detective"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id = 9, Title = "Book 9 Title", Cover = "999",  Content = "bSome content for book 9", Author = "Author5", Genere = "Detective"});
            modelBuilder.Entity<Book>().HasData(new Book() { Id =10, Title ="Book 10 Title", Cover = "000",  Content = "aSome content for book 10", Author = "Author5", Genere = "Detective"});

            {
                int count = 1;
                for(int i = 1; i <= 10; i++)
                {
                    int amount = i << 2 ^ 5;
                    for(int j = 1; j <= amount; j++)
                    {
                        modelBuilder.Entity<Rating>().HasData(new Rating() { Id = count, Score = Math.Max(~j & 5, j & 3), BookID = i});
                        count++;
                    }
                }
            }

            {
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                chars = chars + chars.ToLower();
                Random rnd = new Random(1000);

                int count = 1;
                for(int i = 1; i <= 10; i++)
                {
                    int amount = i & 5;
                    for(int j = 1; j <= amount; j++)
                    {
                        modelBuilder.Entity<Review>().HasData(new Review() 
                        { 
                            Id = count++, 
                            BookID = i,
                            Message = new string(Enumerable.Repeat(chars, rnd.Next(chars.Length)).Select(s => s[rnd.Next(s.Length)]).ToArray()),
                            Reviewer = new string(Enumerable.Repeat(chars, rnd.Next(chars.Length)).Select(s => s[rnd.Next(s.Length)]).ToArray())
                        });
                    }
                }
            }
        }
    }
}