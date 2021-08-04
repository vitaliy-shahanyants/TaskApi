using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApp.Migrations
{
    public partial class AddingOneToManyRelationshipV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Category_CategoryDataid",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryDataid",
                table: "Items",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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

            migrationBuilder.AlterColumn<int>(
                name: "CategoryDataid",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Category_CategoryDataid",
                table: "Items",
                column: "CategoryDataid",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
