using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace accepted.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false),
                    MatchDate = table.Column<DateTime>(type: "Date", nullable: false),
                    MatchTime = table.Column<TimeSpan>(type: "Time", nullable: false),
                    TeamA = table.Column<string>(nullable: false),
                    TeamB = table.Column<string>(nullable: false),
                    Sport = table.Column<decimal>(type: "Numeric(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchOdd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Specifier = table.Column<string>(nullable: false),
                    Odd = table.Column<decimal>(type: "decimal(3, 2)", nullable: false),
                    MatchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchOdd", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchOdd_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Match_MatchDate",
                table: "Match",
                column: "MatchDate");

            migrationBuilder.CreateIndex(
                name: "IX_Match_TeamA",
                table: "Match",
                column: "TeamA");

            migrationBuilder.CreateIndex(
                name: "IX_Match_TeamB",
                table: "Match",
                column: "TeamB");

            migrationBuilder.CreateIndex(
                name: "IX_Match_TeamA_TeamB",
                table: "Match",
                columns: new[] { "TeamA", "TeamB" });

            migrationBuilder.CreateIndex(
                name: "IX_Match_TeamA_TeamB_MatchDate",
                table: "Match",
                columns: new[] { "TeamA", "TeamB", "MatchDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchOdd_MatchId",
                table: "MatchOdd",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchOdd");

            migrationBuilder.DropTable(
                name: "Match");
        }
    }
}
