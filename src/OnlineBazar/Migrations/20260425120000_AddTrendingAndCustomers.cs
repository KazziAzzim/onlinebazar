using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBazar.Migrations
{
    public partial class AddTrendingAndCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItems",
                newName: "UnitPrice");
        }
    }
}
