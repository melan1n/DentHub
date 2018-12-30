using Microsoft.EntityFrameworkCore.Migrations;

namespace DentHub.Data.Migrations
{
    public partial class userPatientFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientRecords",
                columns: table => new
                {
                    PatientId = table.Column<string>(nullable: false),
                    PatientFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRecords", x => new { x.PatientId, x.PatientFileId });
                    table.ForeignKey(
                        name: "FK_PatientRecords_PatientFiles_PatientFileId",
                        column: x => x.PatientFileId,
                        principalTable: "PatientFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientRecords_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_PatientFileId",
                table: "PatientRecords",
                column: "PatientFileId");
        }
    }
}
