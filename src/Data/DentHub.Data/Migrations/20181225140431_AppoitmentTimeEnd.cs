using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DentHub.Data.Migrations
{
    public partial class AppoitmentTimeEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Appointments",
                newName: "TimeStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeEnd",
                table: "Appointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "TimeStart",
                table: "Appointments",
                newName: "Time");
        }
    }
}
