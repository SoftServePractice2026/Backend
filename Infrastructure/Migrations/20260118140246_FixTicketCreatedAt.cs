using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "ticket",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "ticket",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ticket");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "ticket",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");
        }
    }
}
