using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CQRSWithDocker_Identity.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("3d374efc-151a-41ec-973c-7ac58facc759"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("f608b308-912f-489f-99db-d3a615ab74f5"));

            migrationBuilder.RenameColumn(
                name: "response",
                table: "Logger",
                newName: "Response");

            migrationBuilder.RenameColumn(
                name: "request",
                table: "Logger",
                newName: "Request");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Dept", "Name", "Salary" },
                values: new object[,]
                {
                    { new Guid("8252139a-9156-4969-a057-add8898fd4ab"), "IT", "Avdesh", 1899m },
                    { new Guid("ff2992a1-2e39-4c15-befd-17e4862c83e0"), "Dev", "Vikas", 999m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8252139a-9156-4969-a057-add8898fd4ab"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ff2992a1-2e39-4c15-befd-17e4862c83e0"));

            migrationBuilder.RenameColumn(
                name: "Response",
                table: "Logger",
                newName: "response");

            migrationBuilder.RenameColumn(
                name: "Request",
                table: "Logger",
                newName: "request");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Dept", "Name", "Salary" },
                values: new object[,]
                {
                    { new Guid("3d374efc-151a-41ec-973c-7ac58facc759"), "IT", "Avdesh", 1899m },
                    { new Guid("f608b308-912f-489f-99db-d3a615ab74f5"), "Dev", "Vikas", 999m }
                });
        }
    }
}
