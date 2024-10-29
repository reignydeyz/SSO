using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
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
                type: "varbinary(88)",
                maxLength: 88,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers",
                type: "varbinary(88)",
                maxLength: 88,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers");
        }
    }
}
