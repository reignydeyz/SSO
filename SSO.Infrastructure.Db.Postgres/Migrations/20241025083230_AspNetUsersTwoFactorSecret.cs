using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Db.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AspNetUsersTwoFactorSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TwoFactorSecret",
                table: "AspNetUsers",
                type: "bytea",
                maxLength: 88,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers",
                type: "bytea",
                maxLength: 88,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TwoFactorSecret",
                table: "AspNetUsers",
                column: "TwoFactorSecret",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TwoFactorSecret",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers");
        }
    }
}
