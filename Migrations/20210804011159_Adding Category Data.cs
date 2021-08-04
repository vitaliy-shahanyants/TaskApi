using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApp.Migrations
{
    public partial class AddingCategoryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Category_CategoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "CategoryDataid",
                table: "Items",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryDataid",
                table: "Items",
                column: "CategoryDataid");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Category_CategoryDataid",
                table: "Items",
                column: "CategoryDataid",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Category_CategoryDataid",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryDataid",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryDataid",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Category_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
