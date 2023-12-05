using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationAllowedOriginTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationAllowedOrigins",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationAllowedOrigins", x => new { x.ApplicationId, x.Origin });
                    table.ForeignKey(
                        name: "FK_ApplicationAllowedOrigins_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Seed
            migrationBuilder.InsertData(
                table: "ApplicationAllowedOrigins",
                columns: new[] { "ApplicationId", "Origin" },
                values: new object[] { new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), "http://localhost" });

            migrationBuilder.InsertData(
                table: "ApplicationAllowedOrigins",
                columns: new[] { "ApplicationId", "Origin" },
                values: new object[] { new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), "https://localhost" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationAllowedOrigins");
        }
    }
}
