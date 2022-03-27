using Microsoft.EntityFrameworkCore.Migrations;

namespace dashboard_web.Migrations
{
    public partial class Sixth2migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DashboardID",
                table: "Credentials",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dashboard",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_DashboardID",
                table: "Credentials",
                column: "DashboardID");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Dashboard_DashboardID",
                table: "Credentials",
                column: "DashboardID",
                principalTable: "Dashboard",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Dashboard_DashboardID",
                table: "Credentials");

            migrationBuilder.DropTable(
                name: "Dashboard");

            migrationBuilder.DropIndex(
                name: "IX_Credentials_DashboardID",
                table: "Credentials");

            migrationBuilder.DropColumn(
                name: "DashboardID",
                table: "Credentials");
        }
    }
}
