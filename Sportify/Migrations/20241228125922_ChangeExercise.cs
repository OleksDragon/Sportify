using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportify.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Unchanged",
                table: "Exercises",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_UserId",
                table: "Exercises",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Users_UserId",
                table: "Exercises",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Users_UserId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_UserId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Unchanged",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exercises");
        }
    }
}
