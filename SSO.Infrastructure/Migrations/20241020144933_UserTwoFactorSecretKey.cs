using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserTwoFactorSecretKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwoFactorSecretKey",
                table: "AspNetUsers",
                column: "TwoFactorSecretKey",
                unique: true,
                filter: "[TwoFactorSecretKey] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TwoFactorSecretKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers");
        }
    }
}
