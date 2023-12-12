using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDummyApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "ApplicationId", "Name", "TokenExpiration", "RefreshTokenExpiration", "CreatedBy", "DateCreated", "ModifiedBy", "DateModified" },
                values: new object[] { new Guid("3235314a-3ec7-4a4e-b303-3867640d6923"), "dummy", 15, 1 * 60 * 24 * 2, "system", DateTimeOffset.Now, "system", DateTimeOffset.Now });

            #region Dummy permissions
            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("79f900c3-dc6a-44e6-9988-50bba13542c6"), new Guid("3235314a-3ec7-4a4e-b303-3867640d6923"), "app-dummy", "Application dummy" });

            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("79f900c3-dc6a-44e6-9988-50bba13542c7"), new Guid("3235314a-3ec7-4a4e-b303-3867640d6923"), "role-dummy", "Role dummy" });

            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("79f900c3-dc6a-44e6-9988-50bba13542c8"), new Guid("3235314a-3ec7-4a4e-b303-3867640d6923"), "user-dummy", "User dummy" });
            #endregion

            #region User permissions
            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "79f900c3-dc6a-44e6-9988-50bba13542c6", "b54a1e6b-c2ea-43f8-b48f-24db5338554f", "Permission", "app-dummy" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "79f900c3-dc6a-44e6-9988-50bba13542c7", "b54a1e6b-c2ea-43f8-b48f-24db5338554f", "Permission", "role-dummy" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "79f900c3-dc6a-44e6-9988-50bba13542c8", "b54a1e6b-c2ea-43f8-b48f-24db5338554f", "Permission", "user-dummy" });
            #endregion

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ApplicationId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b71c8184-8208-48da-a10b-2376ba211019", new Guid("3235314a-3ec7-4a4e-b303-3867640d6923"), null, "Dummy", "DUMMY" });

            #region Dummy role permissions
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "79f900c3-dc6a-44e6-9988-50bba13542c6", "b71c8184-8208-48da-a10b-2376ba211019", "Permission", "app-dummy" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "79f900c3-dc6a-44e6-9988-50bba13542c7", "b71c8184-8208-48da-a10b-2376ba211019", "Permission", "role-dummy" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "79f900c3-dc6a-44e6-9988-50bba13542c8", "b71c8184-8208-48da-a10b-2376ba211019", "Permission", "user-dummy" });
            #endregion

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b71c8184-8208-48da-a10b-2376ba211019", "b54a1e6b-c2ea-43f8-b48f-24db5338554f" });

            migrationBuilder.InsertData(
                table: "ApplicationCallbacks",
                columns: new[] { "ApplicationId", "Url" },
                values: new object[] { new Guid("3235314a-3ec7-4a4e-b303-3867640d6923"), "https://periapsys.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
