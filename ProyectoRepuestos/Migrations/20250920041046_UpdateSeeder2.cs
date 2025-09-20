using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoRepuestos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeeder2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Repuestos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Repuestos",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Repuestos",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Repuestos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 4, 7, 25, 894, DateTimeKind.Utc).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "Repuestos",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 4, 7, 25, 894, DateTimeKind.Utc).AddTicks(7013));

            migrationBuilder.UpdateData(
                table: "Repuestos",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 20, 4, 7, 25, 894, DateTimeKind.Utc).AddTicks(7015));
        }
    }
}
