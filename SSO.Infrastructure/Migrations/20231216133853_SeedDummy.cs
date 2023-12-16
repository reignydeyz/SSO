using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDummy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region App
            migrationBuilder.InsertData(
                table: "Applications",
                columns: new[] { "ApplicationId", "Name", "TokenExpiration", "RefreshTokenExpiration", "CreatedBy", "DateCreated", "ModifiedBy", "DateModified", "MaxAccessFailedCount" },
                values: new object[] { new Guid("54817ec1-ac27-410f-9fad-a9c79d3f1525"), "dummy", 1 * 60 * 24, 1 * 60 * 24 * 7, "system", DateTimeOffset.Now, "system", DateTimeOffset.Now, 0 });
            #endregion

            #region App permissions
            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("fc4bb40a-72b0-4dcb-ac69-e0783247e143"), new Guid("54817ec1-ac27-410f-9fad-a9c79d3f1525"), "app-dummy-management", "Application dummy management" });

            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("0d7a7a87-08f6-4993-ae1b-a8593d37c6a2"), new Guid("54817ec1-ac27-410f-9fad-a9c79d3f1525"), "role-dummy-management", "Role dummy management" });

            migrationBuilder.InsertData(
                table: "ApplicationPermissions",
                columns: new[] { "PermissionId", "ApplicationId", "Name", "Description" },
                values: new object[] { new Guid("383b72a0-f8db-47ea-b01a-49a405995b9d"), new Guid("54817ec1-ac27-410f-9fad-a9c79d3f1525"), "user-dummy-management", "User dummy management" });
            #endregion

            #region Users
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "FirstName", "LastName" },
                values: new object[] { "53e5d7bd-329e-4807-ba16-3787b43b2b3a", 0, "b261c0de-8223-4ec7-968a-1cb16d32374f", "user@example.com", true, false, null, "USER@EXAMPLE.COM", "USER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEKVOqR5x7eqGQnEzE8M+UUCduKgdEjwhJCT2bsHh/CLFi9g8Ife4NPnvEZFavmR69g==", null, false, "263e72be-ab78-4937-811a-d6cce701eb98", false, "user@example.com", "User", "User" });
            #endregion

            #region User claims
            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "fc4bb40a-72b0-4dcb-ac69-e0783247e143", "53e5d7bd-329e-4807-ba16-3787b43b2b3a", "Permission", "app-dummy-management" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "0d7a7a87-08f6-4993-ae1b-a8593d37c6a2", "53e5d7bd-329e-4807-ba16-3787b43b2b3a", "Permission", "role-dummy-management" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "PermissionId", "UserId", "ClaimType", "ClaimValue" },
                values: new object[] { "383b72a0-f8db-47ea-b01a-49a405995b9d", "53e5d7bd-329e-4807-ba16-3787b43b2b3a", "Permission", "user-dummy-management" });
            #endregion

            #region Roles
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ApplicationId", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "164e313c-b13b-44dd-b715-d5e50f580172", new Guid("54817ec1-ac27-410f-9fad-a9c79d3f1525"), null, "Admin", "ADMIN" });
            #endregion

            #region Admin role claims
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "fc4bb40a-72b0-4dcb-ac69-e0783247e143", "164e313c-b13b-44dd-b715-d5e50f580172", "Permission", "app-dummy-management" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "0d7a7a87-08f6-4993-ae1b-a8593d37c6a2", "164e313c-b13b-44dd-b715-d5e50f580172", "Permission", "role-dummy-management" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "PermissionId", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[] { "383b72a0-f8db-47ea-b01a-49a405995b9d", "164e313c-b13b-44dd-b715-d5e50f580172", "Permission", "user-dummy-management" });
            #endregion

            #region User roles
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "164e313c-b13b-44dd-b715-d5e50f580172", "53e5d7bd-329e-4807-ba16-3787b43b2b3a" });
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
