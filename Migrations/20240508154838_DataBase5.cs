using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentCatalog.Migrations
{
    /// <inheritdoc />
    public partial class DataBase5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Useri_UserModelId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserModelId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Students");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Useri_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "Useri",
                principalColumn: "UserModelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Useri_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserModelId",
                table: "Students",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Useri_UserModelId",
                table: "Students",
                column: "UserModelId",
                principalTable: "Useri",
                principalColumn: "UserModelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
