using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiTaskSchedule.Migrations
{
    public partial class jobdescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Jobs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Jobs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Jobs");
        }
    }
}
