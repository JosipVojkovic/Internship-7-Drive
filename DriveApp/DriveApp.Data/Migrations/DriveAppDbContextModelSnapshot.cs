﻿// <auto-generated />
using System;
using DriveApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DriveApp.Data.Migrations
{
    [DbContext(typeof(DriveAppDbContext))]
    partial class DriveAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DriveApp.Data.Entities.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FileId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "Hello aswell!!",
                            CreatedAt = new DateTime(2024, 12, 23, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(8046),
                            FileId = 12,
                            UserId = 2
                        },
                        new
                        {
                            Id = 2,
                            Content = "Nice project!",
                            CreatedAt = new DateTime(2024, 12, 25, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(8055),
                            FileId = 17,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            Content = "Amazing project!",
                            CreatedAt = new DateTime(2024, 12, 28, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(8057),
                            FileId = 17,
                            UserId = 3
                        },
                        new
                        {
                            Id = 4,
                            Content = "Really interesting diary!",
                            CreatedAt = new DateTime(2024, 12, 26, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(8058),
                            FileId = 23,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ItemType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<DateTime>("LastChanged")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ParentId");

                    b.ToTable("Items");

                    b.HasDiscriminator<string>("ItemType").HasValue("Item");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.SharedItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<int>("SharedUserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("OwnerId");

                    b.ToTable("SharedItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ItemId = 12,
                            OwnerId = 1,
                            SharedUserId = 2
                        },
                        new
                        {
                            Id = 2,
                            ItemId = 3,
                            OwnerId = 1,
                            SharedUserId = 2
                        },
                        new
                        {
                            Id = 3,
                            ItemId = 23,
                            OwnerId = 2,
                            SharedUserId = 1
                        },
                        new
                        {
                            Id = 4,
                            ItemId = 11,
                            OwnerId = 3,
                            SharedUserId = 1
                        });
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "josip.vojkovic@gmail.com",
                            FirstName = "Josip",
                            LastName = "Vojkovic",
                            Password = "12345678"
                        },
                        new
                        {
                            Id = 2,
                            Email = "marko.markovic@gmail.com",
                            FirstName = "Marko",
                            LastName = "Markovic",
                            Password = "Marko123"
                        },
                        new
                        {
                            Id = 3,
                            Email = "ivan.ivic@gmail.com",
                            FirstName = "Ivan",
                            LastName = "Ivic",
                            Password = "Ivan123"
                        });
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.File", b =>
                {
                    b.HasBaseType("DriveApp.Data.Entities.Models.Item");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Folder");

                    b.HasData(
                        new
                        {
                            Id = 12,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7961),
                            Name = "Hello.txt",
                            OwnerId = 1,
                            Content = "Hello world!"
                        },
                        new
                        {
                            Id = 13,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7965),
                            Name = "Goodbye.txt",
                            OwnerId = 1,
                            Content = "Goodbye world!"
                        },
                        new
                        {
                            Id = 14,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7966),
                            Name = "selfie.jpg",
                            OwnerId = 1,
                            ParentId = 2,
                            Content = "A selfie picture in school."
                        },
                        new
                        {
                            Id = 15,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7968),
                            Name = "family.jpg",
                            OwnerId = 1,
                            ParentId = 5,
                            Content = "A family picture on Christmas day."
                        },
                        new
                        {
                            Id = 16,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7969),
                            Name = "brothers.jpg",
                            OwnerId = 1,
                            ParentId = 5,
                            Content = "A picture with my brothers when we were little."
                        },
                        new
                        {
                            Id = 17,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7971),
                            Name = "MarketplaceApp.html",
                            OwnerId = 1,
                            ParentId = 3,
                            Content = "<html><title>MarketplaceApp</title></html>"
                        },
                        new
                        {
                            Id = 18,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7973),
                            Name = "MarketplaceApp.css",
                            OwnerId = 1,
                            ParentId = 3,
                            Content = "*{font-size: 24px; text-align: center}"
                        },
                        new
                        {
                            Id = 19,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7974),
                            Name = "FitnessDBTables.sql",
                            OwnerId = 1,
                            ParentId = 4,
                            Content = "CREATE TABLE users..."
                        },
                        new
                        {
                            Id = 20,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7975),
                            Name = "FitnessDBSeeds.sql",
                            OwnerId = 1,
                            ParentId = 4,
                            Content = "INSERT INTO users..."
                        },
                        new
                        {
                            Id = 21,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7977),
                            Name = "jokes.txt",
                            OwnerId = 2,
                            Content = "If the two chickens crossed the road..."
                        },
                        new
                        {
                            Id = 22,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7978),
                            Name = "text.txt",
                            OwnerId = 2,
                            Content = "Lorem ipsum..."
                        },
                        new
                        {
                            Id = 23,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7979),
                            Name = "document1.txt",
                            OwnerId = 2,
                            ParentId = 6,
                            Content = "Diary about my life..."
                        },
                        new
                        {
                            Id = 24,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7981),
                            Name = "document2.txt",
                            OwnerId = 2,
                            ParentId = 6,
                            Content = "Random motivational quotes..."
                        },
                        new
                        {
                            Id = 25,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7982),
                            Name = "download1.txt",
                            OwnerId = 2,
                            ParentId = 7,
                            Content = "This is a random download from the internet..."
                        },
                        new
                        {
                            Id = 26,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7983),
                            Name = "download2.png",
                            OwnerId = 2,
                            ParentId = 7,
                            Content = "A random png image."
                        },
                        new
                        {
                            Id = 27,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7984),
                            Name = "geometry.pptx",
                            OwnerId = 2,
                            ParentId = 10,
                            Content = "Presentation about geometry."
                        },
                        new
                        {
                            Id = 28,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7985),
                            Name = "numbers.docx",
                            OwnerId = 2,
                            ParentId = 10,
                            Content = "Word document with numbers table."
                        },
                        new
                        {
                            Id = 29,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7988),
                            Name = "operations.pdf",
                            OwnerId = 2,
                            ParentId = 10,
                            Content = "Pdf file with operations explanation."
                        },
                        new
                        {
                            Id = 30,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7989),
                            Name = "randomFile.txt",
                            OwnerId = 3,
                            Content = "This is a random file."
                        },
                        new
                        {
                            Id = 31,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7990),
                            Name = "song1.mp3",
                            OwnerId = 3,
                            ParentId = 11,
                            Content = "Pop music song."
                        },
                        new
                        {
                            Id = 32,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7991),
                            Name = "song2.mp3",
                            OwnerId = 3,
                            ParentId = 11,
                            Content = "Rap music song."
                        });
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.Folder", b =>
                {
                    b.HasBaseType("DriveApp.Data.Entities.Models.Item");

                    b.HasDiscriminator().HasValue("File");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7904),
                            Name = "Projects",
                            OwnerId = 1
                        },
                        new
                        {
                            Id = 2,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7908),
                            Name = "Photos",
                            OwnerId = 1
                        },
                        new
                        {
                            Id = 3,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7910),
                            Name = "MarketplaceApp Project",
                            OwnerId = 1,
                            ParentId = 1
                        },
                        new
                        {
                            Id = 4,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7911),
                            Name = "FitnessDB Project",
                            OwnerId = 1,
                            ParentId = 1
                        },
                        new
                        {
                            Id = 5,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7913),
                            Name = "Family photos",
                            OwnerId = 1,
                            ParentId = 2
                        },
                        new
                        {
                            Id = 6,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7916),
                            Name = "Documents",
                            OwnerId = 2
                        },
                        new
                        {
                            Id = 7,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7917),
                            Name = "Downloads",
                            OwnerId = 2
                        },
                        new
                        {
                            Id = 8,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7918),
                            Name = "School",
                            OwnerId = 2
                        },
                        new
                        {
                            Id = 9,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7920),
                            Name = "English",
                            OwnerId = 2,
                            ParentId = 8
                        },
                        new
                        {
                            Id = 10,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7922),
                            Name = "Maths",
                            OwnerId = 2,
                            ParentId = 8
                        },
                        new
                        {
                            Id = 11,
                            LastChanged = new DateTime(2025, 1, 2, 16, 16, 4, 551, DateTimeKind.Utc).AddTicks(7923),
                            Name = "Music",
                            OwnerId = 3
                        });
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.Comment", b =>
                {
                    b.HasOne("DriveApp.Data.Entities.Models.File", "File")
                        .WithMany("Comments")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DriveApp.Data.Entities.Models.User", "Owner")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.Item", b =>
                {
                    b.HasOne("DriveApp.Data.Entities.Models.User", "Owner")
                        .WithMany("Items")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DriveApp.Data.Entities.Models.Folder", null)
                        .WithMany("Items")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.SharedItem", b =>
                {
                    b.HasOne("DriveApp.Data.Entities.Models.Item", "Item")
                        .WithMany("SharedItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DriveApp.Data.Entities.Models.User", "Owner")
                        .WithMany("SharedItems")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.Item", b =>
                {
                    b.Navigation("SharedItems");
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Items");

                    b.Navigation("SharedItems");
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.File", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("DriveApp.Data.Entities.Models.Folder", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
