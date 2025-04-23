using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskID", "CreatedDate", "Description", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("0b411a9a-0b64-4c70-96b5-f2025ea21ad9"), new DateTime(2024, 5, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Zrobienie formy na lato :)", "Completed", "Zrobienie formy" },
                    { new Guid("56e7ad83-5296-4aa0-82a7-43cb3b9128ed"), new DateTime(2024, 5, 16, 12, 0, 0, 0, DateTimeKind.Utc), "Nauka na kolokwium z Mongo", "InProgress", "Kolokwium bazy" },
                    { new Guid("9b8de07f-ecac-4a14-9943-cb797d47f962"), new DateTime(2024, 5, 19, 12, 0, 0, 0, DateTimeKind.Utc), "Znalezienie pracy jako .NET Developer", "InProgress", "Praca" },
                    { new Guid("a0b9c8d7-e6f5-a4b3-c2d1-e0f9a8b7c6d5"), new DateTime(2024, 5, 18, 12, 0, 0, 0, DateTimeKind.Utc), "Opanować EF Core przed egzaminem", "Pending", "Nauka Entity Framework" },
                    { new Guid("b1c2d3e4-f5a6-b7c8-d9e0-f1a2b3c4d5e6"), new DateTime(2024, 5, 14, 12, 0, 0, 0, DateTimeKind.Utc), "Omówić rozwój kariery .NET", "Completed", "Spotkanie z mentorem" },
                    { new Guid("e2a3c4b5-d6e7-f8a9-b0c1-d2e3f4a5b6c7"), new DateTime(2024, 5, 20, 12, 0, 0, 0, DateTimeKind.Utc), "Kupić mleko, chleb, jajka", "Pending", "Zakupy" },
                    { new Guid("f1b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6"), new DateTime(2024, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc), "Dokończyć projekt Task Tracker", "InProgress", "Projekt ASP.NET" },
                    { new Guid("fde50f74-5bf6-4125-ab35-832a1a987486"), new DateTime(2024, 5, 11, 12, 0, 0, 0, DateTimeKind.Utc), "Bieg na 10 km", "Pending", "Bieganie" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
