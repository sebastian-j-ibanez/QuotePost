using Microsoft.EntityFrameworkCore;
using ProblemAnalysis3.Entities;

namespace ProblemAnalysis3.DataAccess
{
    public class QuoteDbContext : DbContext
    {
        public QuoteDbContext(DbContextOptions<QuoteDbContext> options) : base(options) { }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<QuoteTag> QuoteTags { get; set; }

        // Seed the Db with data on model creation.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Establish many to many relationship between Quote/Tag and QuoteTag objects.
            
            // One Quote to many QuoteTags.
            modelBuilder.Entity<QuoteTag>()
                .HasOne(qt => qt.Quote)
                .WithMany(q => q.QuoteTags)
                .HasForeignKey(qt => qt.QuoteId);

            // One Tag to many QuoteTags.
            modelBuilder.Entity<QuoteTag>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuoteTags)
                .HasForeignKey(qt => qt.TagId);

            // Seed the database with starting data.
            modelBuilder.Entity<Quote>().HasData(
                new Quote() { 
                    QuoteId = 1, 
                    Content = "The greatest obstacle to living is expectation, which depends on tomorrow and wastes today.",
                    Author = "Seneca"
                },
                new Quote()
                {
                    QuoteId = 2,
                    Content = "It is not death that a man should fear, but he should fear never beginning to live.",
                    Author = "Marcus Aurelius"
                },
                new Quote()
                {
                    QuoteId = 3,
                    Content = "Peace comes from within. Do not seek it without.",
                    Author = "Siddharta Gautama"
                }
            );

            modelBuilder.Entity<Tag>().HasData(
                new Tag() { TagId = 1, TagName = "Stoicism" },
                new Tag() { TagId = 2, TagName = "Buddhism" }
            );

            modelBuilder.Entity<QuoteTag>().HasData(
                new QuoteTag() { QuoteId = 1, TagId = 1 },
                new QuoteTag() { QuoteId = 2, TagId = 1 },
                new QuoteTag() { QuoteId = 3, TagId = 2 }
            );
        }
    }
}
