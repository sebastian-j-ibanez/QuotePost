﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProblemAnalysis3.DataAccess;

#nullable disable

namespace ProblemAnalysis3.Migrations
{
    [DbContext(typeof(QuoteDbContext))]
    [Migration("20240320183519_InitDb")]
    partial class InitDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProblemAnalysis3.Entities.Quote", b =>
                {
                    b.Property<int>("QuoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuoteId"));

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuoteId");

                    b.ToTable("Quotes");

                    b.HasData(
                        new
                        {
                            QuoteId = 1,
                            Author = "Seneca",
                            Content = "The greatest obstacle to living is expectation, which depends on tomorrow and wastes today."
                        },
                        new
                        {
                            QuoteId = 2,
                            Author = "Marcus Aurelius",
                            Content = "It is not death that a man should fear, but he should fear never beginning to live."
                        },
                        new
                        {
                            QuoteId = 3,
                            Author = "Siddharta Gautama",
                            Content = "Peace comes from within. Do not seek it without."
                        });
                });

            modelBuilder.Entity("ProblemAnalysis3.Entities.QuoteTag", b =>
                {
                    b.Property<int>("QuoteId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("QuoteId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("QuoteTags");

                    b.HasData(
                        new
                        {
                            QuoteId = 1,
                            TagId = 1
                        },
                        new
                        {
                            QuoteId = 2,
                            TagId = 1
                        },
                        new
                        {
                            QuoteId = 3,
                            TagId = 2
                        });
                });

            modelBuilder.Entity("ProblemAnalysis3.Entities.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("TagName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            TagId = 1,
                            TagName = "Stoicism"
                        },
                        new
                        {
                            TagId = 2,
                            TagName = "Buddhism"
                        });
                });

            modelBuilder.Entity("ProblemAnalysis3.Entities.QuoteTag", b =>
                {
                    b.HasOne("ProblemAnalysis3.Entities.Quote", "Quote")
                        .WithMany("QuoteTags")
                        .HasForeignKey("QuoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProblemAnalysis3.Entities.Tag", "Tag")
                        .WithMany("QuoteTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quote");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("ProblemAnalysis3.Entities.Quote", b =>
                {
                    b.Navigation("QuoteTags");
                });

            modelBuilder.Entity("ProblemAnalysis3.Entities.Tag", b =>
                {
                    b.Navigation("QuoteTags");
                });
#pragma warning restore 612, 618
        }
    }
}
