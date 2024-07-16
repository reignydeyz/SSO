using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RealmIdpSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RealmIdpSettings",
                columns: table => new
                {
                    RealmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityProvider = table.Column<byte>(type: "tinyint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealmIdpSettings", x => new { x.RealmId, x.IdentityProvider });
                    table.ForeignKey(
                        name: "FK_RealmIdpSettings_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "RealmId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealmIdpSettings");
        }
    }
}
