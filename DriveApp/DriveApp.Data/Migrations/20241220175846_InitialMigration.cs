using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DriveApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    LastChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Items_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    SharedUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedItems_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SharedItemId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_SharedItems_SharedItemId",
                        column: x => x.SharedItemId,
                        principalTable: "SharedItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password" },
                values: new object[,]
                {
                    { 1, "josip.vojkovic@gmail.com", "Josip", "Vojkovic", "12345678" },
                    { 2, "marko.markovic@gmail.com", "Marko", "Markovic", "Marko123" },
                    { 3, "ivan.ivic@gmail.com", "Ivan", "Ivic", "Ivan123" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "ItemType", "LastChanged", "Name", "OwnerId", "ParentId" },
                values: new object[,]
                {
                    { 1, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2414), "Projects", 1, null },
                    { 2, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2417), "Photos", 1, null },
                    { 6, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2425), "Documents", 2, null },
                    { 7, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2426), "Downloads", 2, null },
                    { 8, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2428), "School", 2, null },
                    { 11, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2438), "Music", 3, null }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Content", "ItemType", "LastChanged", "Name", "OwnerId", "ParentId" },
                values: new object[,]
                {
                    { 12, "Hello world!", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2470), "Hello.txt", 1, null },
                    { 13, "Goodbye world!", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2473), "Goodbye.txt", 1, null },
                    { 21, "If the two chickens crossed the road...", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2485), "jokes.txt", 2, null },
                    { 22, "Lorem ipsum...", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2486), "text.txt", 2, null },
                    { 30, "This is a random file.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2496), "randomFile.txt", 3, null }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "ItemType", "LastChanged", "Name", "OwnerId", "ParentId" },
                values: new object[,]
                {
                    { 3, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2419), "MarketplaceApp Project", 1, 1 },
                    { 4, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2421), "FitnessDB Project", 1, 1 },
                    { 5, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2422), "Family photos", 1, 2 },
                    { 9, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2435), "English", 2, 8 },
                    { 10, "File", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2437), "Maths", 2, 8 }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Content", "ItemType", "LastChanged", "Name", "OwnerId", "ParentId" },
                values: new object[,]
                {
                    { 14, "A selfie picture in school.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2474), "selfie.jpg", 1, 2 },
                    { 23, "Diary about my life...", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2487), "document1.txt", 2, 6 },
                    { 24, "Random motivational quotes...", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2488), "document2.txt", 2, 6 },
                    { 25, "This is a random download from the internet...", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2489), "download1.txt", 2, 7 },
                    { 26, "A random png image.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2490), "download2.png", 2, 7 },
                    { 31, "Pop music song.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2497), "song1.mp3", 3, 11 },
                    { 32, "Rap music song.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2498), "song2.mp3", 3, 11 }
                });

            migrationBuilder.InsertData(
                table: "SharedItems",
                columns: new[] { "Id", "ItemId", "OwnerId", "SharedUserId" },
                values: new object[,]
                {
                    { 1, 12, 1, 2 },
                    { 4, 11, 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "SharedItemId", "UserId" },
                values: new object[] { 1, "Hello aswell!!", new DateTime(2024, 12, 10, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2552), 1, 2 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Content", "ItemType", "LastChanged", "Name", "OwnerId", "ParentId" },
                values: new object[,]
                {
                    { 15, "A family picture on Christmas day.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2476), "family.jpg", 1, 5 },
                    { 16, "A picture with my brothers when we were little.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2477), "brothers.jpg", 1, 5 },
                    { 17, "<html><title>MarketplaceApp</title></html>", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2479), "MarketplaceApp.html", 1, 3 },
                    { 18, "*{font-size: 24px; text-align: center}", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2480), "MarketplaceApp.css", 1, 3 },
                    { 19, "CREATE TABLE users...", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2482), "FitnessDBTables.sql", 1, 4 },
                    { 20, "INSERT INTO users...", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2483), "FitnessDBSeeds.sql", 1, 4 },
                    { 27, "Presentation about geometry.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2491), "geometry.pptx", 2, 10 },
                    { 28, "Word document with numbers table.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2493), "numbers.docx", 2, 10 },
                    { 29, "Pdf file with operations explanation.", "Folder", new DateTime(2024, 12, 20, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2495), "operations.pdf", 2, 10 }
                });

            migrationBuilder.InsertData(
                table: "SharedItems",
                columns: new[] { "Id", "ItemId", "OwnerId", "SharedUserId" },
                values: new object[,]
                {
                    { 2, 3, 1, 2 },
                    { 3, 23, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "SharedItemId", "UserId" },
                values: new object[,]
                {
                    { 2, "Nice project!", new DateTime(2024, 12, 12, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2563), 2, 2 },
                    { 3, "Amazing project!", new DateTime(2024, 12, 15, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2564), 2, 3 },
                    { 4, "Really interesting diary!", new DateTime(2024, 12, 13, 17, 58, 45, 704, DateTimeKind.Utc).AddTicks(2566), 3, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SharedItemId",
                table: "Comments",
                column: "SharedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OwnerId",
                table: "Items",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentId",
                table: "Items",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItems_ItemId",
                table: "SharedItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedItems_OwnerId",
                table: "SharedItems",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "SharedItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
