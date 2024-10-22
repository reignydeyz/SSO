using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Db.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSAspNetUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing index on the old column
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TwoFactorSecretKey",
                table: "AspNetUsers");

            // Rename the existing column to the new name
            migrationBuilder.RenameColumn(
                name: "TwoFactorSecretKey",
                table: "AspNetUsers",
                newName: "TwoFactorSecretKeySalt");

            // Add the new column for the hashed secret key
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecretKeyHash",
                table: "AspNetUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            // Create the index on the new column using raw SQL
            migrationBuilder.Sql(@"
        CREATE UNIQUE INDEX ""IX_AspNetUsers_TwoFactorSecretKeyHash"" 
        ON ""AspNetUsers"" (""TwoFactorSecretKeyHash"") 
        WHERE ""TwoFactorSecretKeyHash"" IS NOT NULL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the index on the new column first
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_AspNetUsers_TwoFactorSecretKeyHash\";");

            // Drop the new column
            migrationBuilder.DropColumn(
                name: "TwoFactorSecretKeyHash",
                table: "AspNetUsers");

            // Rename the column back to the original name
            migrationBuilder.RenameColumn(
                name: "TwoFactorSecretKeySalt",
                table: "AspNetUsers",
                newName: "TwoFactorSecretKey");

            // Recreate the index on the original column
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TwoFactorSecretKey",
                table: "AspNetUsers",
                column: "TwoFactorSecretKey",
                unique: true,
                filter: "TwoFactorSecretKey IS NOT NULL");
        }
    }
}
