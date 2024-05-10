using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentCatalog.Migrations
{
    /// <inheritdoc />
    public partial class migrare2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Useri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordConfirm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Useri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cursuri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursuri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cursuri_Useri_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Useri",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mesaje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesaje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesaje_Useri_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Useri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Mesaje_Useri_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Useri",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    YearOfStudy = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    IsEnrolled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studenti_Useri_UserId",
                        column: x => x.UserId,
                        principalTable: "Useri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CursuriStudenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    Grade = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursuriStudenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CursuriStudenti_Cursuri_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Cursuri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CursuriStudenti_Studenti_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cursuri_TeacherId",
                table: "Cursuri",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CursuriStudenti_CourseId",
                table: "CursuriStudenti",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CursuriStudenti_StudentId",
                table: "CursuriStudenti",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesaje_ReceiverId",
                table: "Mesaje",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesaje_SenderId",
                table: "Mesaje",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_UserId",
                table: "Studenti",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CursuriStudenti");

            migrationBuilder.DropTable(
                name: "Grupe");

            migrationBuilder.DropTable(
                name: "Mesaje");

            migrationBuilder.DropTable(
                name: "Cursuri");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Useri");
        }
    }
}
