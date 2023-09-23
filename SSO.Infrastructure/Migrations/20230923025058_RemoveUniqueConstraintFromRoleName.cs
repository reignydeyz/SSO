using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintFromRoleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX RoleNameIndex ON AspNetRoles;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
