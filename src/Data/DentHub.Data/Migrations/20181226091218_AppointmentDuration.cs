using Microsoft.EntityFrameworkCore.Migrations;

namespace DentHub.Data.Migrations
{
    public partial class AppointmentDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Appointments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Duration",
                table: "Appointments",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
