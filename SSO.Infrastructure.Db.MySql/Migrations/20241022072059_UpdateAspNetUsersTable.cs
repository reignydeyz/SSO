using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Db.MySql.Migrations
{
    public partial class UpdateAspNetUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Begin transaction
            migrationBuilder.Sql("START TRANSACTION;");

            // Drop the old index
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TwoFactorSecretKey",
                table: "AspNetUsers");

            // Commit after dropping the index
            migrationBuilder.Sql("COMMIT;");

            // Begin transaction for renaming column with raw SQL
            migrationBuilder.Sql("START TRANSACTION;");

            // Rename the existing column using raw SQL
            migrationBuilder.Sql("ALTER TABLE AspNetUsers RENAME COLUMN TwoFactorSecretKey TO TwoFactorSecretKeySalt;");

            // Commit after renaming the column
            migrationBuilder.Sql("COMMIT;");

            // Begin transaction for adding new column
            migrationBuilder.Sql("START TRANSACTION;");

            // Add the new column
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecretKeyHash",
                table: "AspNetUsers",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            // Commit after adding the new column
            migrationBuilder.Sql("COMMIT;");

            // Begin transaction for creating new index
            migrationBuilder.Sql("START TRANSACTION;");

            // Create a new index for the new column
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TwoFactorSecretKeyHash",
                table: "AspNetUsers",
                column: "TwoFactorSecretKeyHash",
                unique: true);

            // Commit after creating the new index
            migrationBuilder.Sql("COMMIT;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Begin transaction
            migrationBuilder.Sql("START TRANSACTION;");

            // Drop the new index
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TwoFactorSecretKeyHash",
                table: "AspNetUsers");

            // Commit after dropping the index
            migrationBuilder.Sql("COMMIT;");

            // Begin transaction for dropping the new column
            migrationBuilder.Sql("START TRANSACTION;");

            // Drop the new column
            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKeyHash",
                table: "AspNetUsers");

            // Commit after dropping the new column
            migrationBuilder.Sql("COMMIT;");

            // Begin transaction for renaming the column back using raw SQL
            migrationBuilder.Sql("START TRANSACTION;");

            // Rename the column back to the original name using raw SQL
            migrationBuilder.Sql("ALTER TABLE AspNetUsers RENAME COLUMN TwoFactorSecretKeySalt TO TwoFactorSecretKey;");

            // Commit after renaming the column back
            migrationBuilder.Sql("COMMIT;");

            // Begin transaction for recreating the old index
            migrationBuilder.Sql("START TRANSACTION;");

            // Recreate the old index
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TwoFactorSecretKey",
                table: "AspNetUsers",
                column: "TwoFactorSecretKey",
                unique: true);

            // Commit after recreating the old index
            migrationBuilder.Sql("COMMIT;");
        }
    }
}