using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiTaskSchedule.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "(newsequentialid())"),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PercentCompleted = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOutputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "(newsequentialid())"),
                    JobId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Job_JobOutputs",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOutputs_JobId",
                table: "JobOutputs",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOutputs");

            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
