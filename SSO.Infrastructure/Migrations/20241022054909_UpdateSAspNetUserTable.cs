using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSAspNetUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TwoFactorSecretKey",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers",
                newName: "TwoFactorSecretKeySalt");

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecretKeyHash",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwoFactorSecretKey",
                table: "AspNetUsers",
                column: "TwoFactorSecretKeyHash",
                unique: true,
                filter: "[TwoFactorSecretKeyHash] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TwoFactorSecretKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKeyHash",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TwoFactorSecretKeySalt",
                table: "AspNetUsers",
                newName: "TwoFactorSecretKey");

            migrationBuilder.CreateIndex(
                name: "IX_TwoFactorSecretKey",
                table: "AspNetUsers",
                column: "TwoFactorSecretKey",
                unique: true,
                filter: "[TwoFactorSecretKey] IS NOT NULL");
        }
    }
}
