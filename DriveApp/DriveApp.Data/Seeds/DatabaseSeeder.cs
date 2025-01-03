using DriveApp.Data.Entities.Models;
using DriveApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveApp.Data.Seeds
{
    public static class DatabaseSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasData(new List<User>
                {
                    new User("josip.vojkovic@gmail.com", "12345678", "Josip", "Vojkovic")
                    {
                        Id = 1
                    },
                    new User("marko.markovic@gmail.com", "Marko123", "Marko", "Markovic")
                    {
                        Id = 2
                    },
                    new User("ivan.ivic@gmail.com", "Ivan123", "Ivan", "Ivic")
                    {
                        Id = 3
                    }
                });

            builder.Entity<Folder>()
                .HasData(new List<Folder>
                {
                    // User id = 1
                    new Folder("Projects", null, 1)
                    {
                        Id = 1
                    },
                    new Folder("Photos", null, 1)
                    {
                        Id = 2
                    },
                    new Folder("MarketplaceApp Project", 1, 1)
                    {
                        Id = 3
                    },
                    new Folder("FitnessDB Project", 1, 1)
                    {
                        Id = 4
                    },
                    new Folder("Family photos", 2, 1)
                    {
                        Id= 5
                    },
                    // User id = 2
                    new Folder("Documents", null, 2)
                    {
                        Id = 6
                    },
                    new Folder("Downloads", null, 2)
                    {
                        Id = 7
                    },
                    new Folder("School", null, 2)
                    {
                        Id = 8
                    },
                    new Folder("English", 8, 2)
                    {
                        Id = 9
                    },
                    new Folder("Maths", 8, 2)
                    {
                        Id= 10
                    },
                    // User id = 3
                    new Folder("Music", null, 3)
                    {
                        Id = 11
                    }
                });

            builder.Entity<Entities.Models.File>()
                .HasData(new List<Entities.Models.File>
                {
                    // User id = 1
                    new Entities.Models.File("Hello.txt", null, 1, "Hello world!")
                    {
                        Id = 12
                    },
                    new Entities.Models.File("Goodbye.txt", null, 1, "Goodbye world!")
                    {
                        Id = 13
                    },
                    new Entities.Models.File("selfie.jpg", 2, 1, "A selfie picture in school.")
                    {
                        Id = 14
                    },
                    new Entities.Models.File("family.jpg", 5, 1, "A family picture on Christmas day.")
                    {
                        Id = 15
                    },
                    new Entities.Models.File("brothers.jpg", 5, 1, "A picture with my brothers when we were little.")
                    {
                        Id = 16
                    },
                    new Entities.Models.File("MarketplaceApp.html", 3, 1, "<html><title>MarketplaceApp</title></html>")
                    {
                        Id = 17
                    },
                    new Entities.Models.File("MarketplaceApp.css", 3, 1, "*{font-size: 24px; text-align: center}")
                    {
                        Id = 18
                    },
                    new Entities.Models.File("FitnessDBTables.sql", 4, 1, "CREATE TABLE users...")
                    {
                        Id = 19
                    },
                    new Entities.Models.File("FitnessDBSeeds.sql", 4, 1, "INSERT INTO users...")
                    {
                        Id = 20
                    },
                    // User id = 2
                    new Entities.Models.File("jokes.txt", null, 2, "If the two chickens crossed the road...")
                    {
                        Id = 21
                    },
                    new Entities.Models.File("text.txt", null, 2, "Lorem ipsum...")
                    {
                        Id = 22
                    },
                    new Entities.Models.File("document1.txt", 6, 2, "Diary about my life...")
                    {
                        Id = 23
                    },
                    new Entities.Models.File("document2.txt", 6, 2, "Random motivational quotes...")
                    {
                        Id = 24
                    },
                    new Entities.Models.File("download1.txt", 7, 2, "This is a random download from the internet...")
                    {
                        Id = 25
                    },
                    new Entities.Models.File("download2.png", 7, 2, "A random png image.")
                    {
                        Id = 26
                    },
                    new Entities.Models.File("geometry.pptx", 10, 2, "Presentation about geometry.")
                    {
                        Id = 27
                    },
                    new Entities.Models.File("numbers.docx", 10, 2, "Word document with numbers table.")
                    {
                        Id = 28
                    },
                    new Entities.Models.File("operations.pdf", 10, 2, "Pdf file with operations explanation.")
                    {
                        Id = 29
                    },
                    // User id = 3
                    new Entities.Models.File("randomFile.txt", null, 3, "This is a random file.")
                    {
                        Id = 30
                    },
                    new Entities.Models.File("song1.mp3", 11, 3, "Pop music song.")
                    {
                        Id = 31
                    },
                    new Entities.Models.File("song2.mp3", 11, 3, "Rap music song.")
                    {
                        Id = 32
                    }
                });

            builder.Entity<SharedItem>()
                .HasData(new List<SharedItem>
                {
                    new SharedItem(12, 1, 2)
                    {
                        Id = 1
                    },
                    new SharedItem(3, 1, 2)
                    {
                        Id = 2
                    },
                    new SharedItem(23, 2, 1)
                    {
                        Id = 3
                    },
                    new SharedItem(11, 3, 1)
                    {
                        Id = 4
                    }
                });

            builder.Entity<Comment>()
                .HasData(new List<Comment>
                {
                    new Comment(12, 2, "Hello aswell!!", DateTime.UtcNow.AddDays(-10))
                    {
                        Id= 1
                    },
                    new Comment(17, 2, "Nice project!", DateTime.UtcNow.AddDays(-8))
                    {
                        Id= 2
                    },
                    new Comment(17, 3, "Amazing project!", DateTime.UtcNow.AddDays(-5))
                    {
                        Id= 3
                    },
                    new Comment(23, 1, "Really interesting diary!", DateTime.UtcNow.AddDays(-7))
                    {
                        Id= 4
                    }
                });
        }
    }
}
