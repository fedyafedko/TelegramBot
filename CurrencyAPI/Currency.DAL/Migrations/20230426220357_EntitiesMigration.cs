using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Currency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    FromCurrency = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amout = table.Column<double>(type: "float", nullable: false),
                    Result = table.Column<double>(type: "float", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.FromCurrency);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
