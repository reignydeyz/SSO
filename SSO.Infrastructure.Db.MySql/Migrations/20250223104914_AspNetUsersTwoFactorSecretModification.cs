using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Db.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AspNetUsersTwoFactorSecretModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<byte[]>(
                name: "TwoFactorSecret",
                table: "AspNetUsers",
                type: "varbinary(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(88)",
                oldMaxLength: 88,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "TwoFactorSecret",
                table: "AspNetUsers",
                type: "varbinary(88)",
                maxLength: 88,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers",
                type: "varbinary(88)",
                maxLength: 88,
                nullable: true);
        }
    }
}
