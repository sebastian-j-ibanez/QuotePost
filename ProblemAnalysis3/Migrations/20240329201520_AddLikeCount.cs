using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProblemAnalysis3.Migrations
{
    /// <inheritdoc />
    public partial class AddLikeCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Quotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Quotes",
                keyColumn: "QuoteId",
                keyValue: 1,
                column: "LikeCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Quotes",
                keyColumn: "QuoteId",
                keyValue: 2,
                column: "LikeCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Quotes",
                keyColumn: "QuoteId",
                keyValue: 3,
                column: "LikeCount",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Quotes");
        }
    }
}
