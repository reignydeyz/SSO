using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "ApplicationId", "Name", "TokenExpiration", "RefreshTokenExpiration", "CreatedBy", "DateCreated", "ModifiedBy", "DateModified" },
                values: new object[] { new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), "root", 1 * 60 * 24, 1 * 60 * 24 * 7, "system", DateTimeOffset.Now, "system", DateTimeOffset.Now });

            #region Root permissions
            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), "app-management", "Application management" });

            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("69f900c3-dc6a-44e6-9988-50bba13542c7"), new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), "role-management", "Role management" });

            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("69f900c3-dc6a-44e6-9988-50bba13542c8"), new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), "user-management", "User management" });
            #endregion

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b54a1e6b-c2ea-43f8-b48f-24db5338554f", 0, "b261c0de-8223-4ec7-968a-1cb16d32374f", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEKVOqR5x7eqGQnEzE8M+UUCduKgdEjwhJCT2bsHh/CLFi9g8Ife4NPnvEZFavmR69g==", null, false, "263e72be-ab78-4937-811a-d6cce701eb98", false, "admin@example.com" });

            #region User permissions
            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "69f900c3-dc6a-44e6-9988-50bba13542c6", "b54a1e6b-c2ea-43f8-b48f-24db5338554f", "Permission", "app-management" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "69f900c3-dc6a-44e6-9988-50bba13542c7", "b54a1e6b-c2ea-43f8-b48f-24db5338554f", "Permission", "role-management" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "69f900c3-dc6a-44e6-9988-50bba13542c8", "b54a1e6b-c2ea-43f8-b48f-24db5338554f", "Permission", "user-management" });
            #endregion

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ApplicationId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "63881191-d2e8-49fd-a42e-b82eff79da17", new Guid("69f900c3-dc6a-44e6-9988-50bba13542c6"), null, "Admin", "ADMIN" });

            #region Root admin permissions
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "69f900c3-dc6a-44e6-9988-50bba13542c6", "63881191-d2e8-49fd-a42e-b82eff79da17", "Permission", "app-management" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "69f900c3-dc6a-44e6-9988-50bba13542c6", "63881191-d2e8-49fd-a42e-b82eff79da17", "Permission", "role-management" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "69f900c3-dc6a-44e6-9988-50bba13542c6", "63881191-d2e8-49fd-a42e-b82eff79da17", "Permission", "user-management" });
            #endregion

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "63881191-d2e8-49fd-a42e-b82eff79da17", "b54a1e6b-c2ea-43f8-b48f-24db5338554f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
