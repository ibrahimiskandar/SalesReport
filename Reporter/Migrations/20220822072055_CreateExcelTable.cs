using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reporter.Migrations
{
    public partial class CreateExcelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesTable",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Segment = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    DiscountBand = table.Column<string>(nullable: true),
                    UnitSold = table.Column<decimal>(nullable: false),
                    ManufacturingPrice = table.Column<decimal>(nullable: false),
                    SalePrice = table.Column<decimal>(nullable: false),
                    GrossSales = table.Column<decimal>(nullable: false),
                    Discounts = table.Column<decimal>(nullable: false),
                    Sales = table.Column<decimal>(nullable: false),
                    COGS = table.Column<decimal>(nullable: false),
                    Profit = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTable", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesTable");
        }
    }
}
