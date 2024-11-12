using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sportify.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeToWorkouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Complexity",
                table: "Workouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutTypeId",
                table: "Workouts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkoutTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WorkoutTypes",
                columns: new[] { "Id", "Description", "ImageBase64", "Title" },
                values: new object[,]
                {
                    { 1, "Вправи, що підвищують пульс і покращують роботу серця та легенів. Приклади: біг, ходьба, плавання, велоспорт, стрибки на скакалці. Вони допомагають спалювати калорії, підвищують витривалість і зміцнюють серцево-судинну систему.", "Згодом!!!!", "Кардіо" },
                    { 2, "Тренування, спрямовані на зміцнення м''язів та підвищення їх сили. Основні види: вправи з вагою тіла (віджимання, присідання), заняття з гантелями, штангою або на тренажерах. Силові вправи допомагають наростити м''язову масу, покращують обмін речовин та підвищують загальну фізичну витривалість.", "Згодом!!!!", "Силове" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_WorkoutTypeId",
                table: "Workouts",
                column: "WorkoutTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_WorkoutTypes_WorkoutTypeId",
                table: "Workouts",
                column: "WorkoutTypeId",
                principalTable: "WorkoutTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_WorkoutTypes_WorkoutTypeId",
                table: "Workouts");

            migrationBuilder.DropTable(
                name: "WorkoutTypes");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_WorkoutTypeId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Complexity",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "WorkoutTypeId",
                table: "Workouts");
        }
    }
}
