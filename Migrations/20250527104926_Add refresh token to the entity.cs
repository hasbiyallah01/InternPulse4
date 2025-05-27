using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternPulse4.Migrations
{
    /// <inheritdoc />
    public partial class Addrefreshtokentotheentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RememberMe",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RememberMe" },
                values: new object[] { new DateTime(2025, 5, 27, 10, 49, 26, 123, DateTimeKind.Utc).AddTicks(1271), "$2a$11$7mAWMsrUZ09NOQguZiBUEOHjacRLX9a/8b6YoJOgRKCEsndK8kZKa", null, null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RememberMe",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2025, 5, 26, 21, 41, 30, 556, DateTimeKind.Utc).AddTicks(9407), "$2a$11$3QR8RzVU3PG94bQsWu/v0eAEOuOchge8tUSVaDKiK6Hd0A4XfDHgG" });
        }
    }
}
