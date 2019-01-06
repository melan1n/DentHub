using Microsoft.EntityFrameworkCore.Migrations;

namespace DentHub.Data.Migrations
{
    public partial class removeRatingForAppointmentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Appointments_AppointmentId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ForAppointmentId",
                table: "Ratings");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Ratings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Appointments_AppointmentId",
                table: "Ratings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Appointments_AppointmentId",
                table: "Ratings");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Ratings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ForAppointmentId",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Appointments_AppointmentId",
                table: "Ratings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
