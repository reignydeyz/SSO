﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSO.Infrastructure.Db.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RealmsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Realms",
                columns: table => new
                {
                    RealmId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() AT TIME ZONE 'UTC'"),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() AT TIME ZONE 'UTC'"),
                    ModifiedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateInactive = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realms", x => x.RealmId);
                });

            #region Realms data
            migrationBuilder.InsertData(
                table: "Realms",
                columns: new[] { "RealmId", "Name", "CreatedBy", "DateCreated", "ModifiedBy", "DateModified" },
                values: new object[] { new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"), "Default", "system", DateTimeOffset.Now, "system", DateTimeOffset.Now });
            #endregion

            migrationBuilder.DropIndex(
                name: "IX_Groups_Name",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Applications_Name",
                table: "Applications");

            migrationBuilder.AddColumn<Guid>(
                name: "RealmId",
                table: "Groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Applications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Applications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Applications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "RealmId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));

            migrationBuilder.CreateTable(
                name: "RealmUsers",
                columns: table => new
                {
                    RealmId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealmUsers", x => new { x.RealmId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RealmUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealmUsers_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "RealmId",
                        onDelete: ReferentialAction.Restrict);
                });

            #region RealmUsers data
            migrationBuilder.Sql(@"
                INSERT INTO ""RealmUsers"" (""RealmId"", ""UserId"")
                SELECT 'afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa', ""Id""
                FROM ""AspNetUsers"";
            ");
            #endregion

            migrationBuilder.CreateIndex(
                name: "IX_Groups_RealmId_Name",
                table: "Groups",
                columns: new[] { "RealmId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RealmId_Name",
                table: "Applications",
                columns: new[] { "RealmId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Realms_Name",
                table: "Realms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealmUsers_UserId",
                table: "RealmUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Realms_RealmId",
                table: "Applications",
                column: "RealmId",
                principalTable: "Realms",
                principalColumn: "RealmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Realms_RealmId",
                table: "Groups",
                column: "RealmId",
                principalTable: "Realms",
                principalColumn: "RealmId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Realms_RealmId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Realms_RealmId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "RealmUsers");

            migrationBuilder.DropTable(
                name: "Realms");

            migrationBuilder.DropIndex(
                name: "IX_Groups_RealmId_Name",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Applications_RealmId_Name",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "RealmId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "RealmId",
                table: "Applications");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Applications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Applications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Applications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name",
                table: "Groups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Name",
                table: "Applications",
                column: "Name",
                unique: true);
        }
    }
}