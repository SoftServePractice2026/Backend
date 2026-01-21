using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketCreatedAt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedAt",
                table: "ticket",
                newName: "last_modified_at");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "ticket",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_modified_at",
                table: "ticket",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_modified_at",
                table: "ticket",
                newName: "LastModifiedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "ticket",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedAt",
                table: "ticket",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);
        }
    }
}
