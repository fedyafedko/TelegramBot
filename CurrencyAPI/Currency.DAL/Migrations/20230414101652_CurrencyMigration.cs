using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Currency.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amout = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
