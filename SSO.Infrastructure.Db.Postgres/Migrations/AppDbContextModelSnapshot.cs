﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SSO.Infrastructure.Db.Postgres;

#nullable disable

namespace SSO.Infrastructure.Db.Postgres.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.Application", b =>
                {
                    b.Property<Guid>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<DateTime?>("DateInactive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<short>("MaxAccessFailedCount")
                        .HasColumnType("smallint");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("RealmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));

                    b.Property<int>("RefreshTokenExpiration")
                        .HasColumnType("integer");

                    b.Property<int>("TokenExpiration")
                        .HasColumnType("integer");

                    b.HasKey("ApplicationId");

                    b.HasIndex("RealmId", "Name")
                        .IsUnique();

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationCallback", b =>
                {
                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("ApplicationId", "Url");

                    b.ToTable("ApplicationCallbacks");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationPermission", b =>
                {
                    b.Property<Guid>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("PermissionId");

                    b.HasIndex("ApplicationId", "Name")
                        .IsUnique();

                    b.ToTable("ApplicationPermissions");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.HasIndex("ApplicationId", "Name")
                        .IsUnique();

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasDefaultValue("admin");

                    b.Property<DateTime?>("DateConfirmed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<DateTime?>("DateInactive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasDefaultValue("admin");

                    b.Property<DateTime?>("LastFailedPasswordAttempt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasDefaultValue("admin");

                    b.Property<DateTime?>("LastPasswordChanged")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastSessionId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasDefaultValue("35c7c988-7c48-4f13-bf41-4edbd060a394");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasDefaultValue("admin");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("PasswordExpiry")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<byte[]>("TwoFactorSecret")
                        .HasMaxLength(88)
                        .HasColumnType("bytea");

                    b.Property<byte[]>("TwoFactorSecretKey")
                        .HasMaxLength(88)
                        .HasColumnType("bytea");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("TwoFactorSecret")
                        .IsUnique();

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.Group", b =>
                {
                    b.Property<Guid>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<DateTime?>("DateInactive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("RealmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));

                    b.HasKey("GroupId");

                    b.HasIndex("RealmId", "Name")
                        .IsUnique();

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SSO.Domain.Models.GroupRole", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("GroupId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("GroupRoles");
                });

            modelBuilder.Entity("SSO.Domain.Models.GroupUser", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("SSO.Domain.Models.Realm", b =>
                {
                    b.Property<Guid>("RealmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<DateTime?>("DateInactive")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("RealmId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Realms");
                });

            modelBuilder.Entity("SSO.Domain.Models.RealmIdpSettings", b =>
                {
                    b.Property<Guid>("RealmId")
                        .HasColumnType("uuid");

                    b.Property<short>("IdentityProvider")
                        .HasColumnType("smallint");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RealmId", "IdentityProvider");

                    b.ToTable("RealmIdpSettings");
                });

            modelBuilder.Entity("SSO.Domain.Models.RealmUser", b =>
                {
                    b.Property<Guid>("RealmId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("RealmId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("RealmUsers");
                });

            modelBuilder.Entity("SSO.Domain.Models.RootUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("RootUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SSO.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("SSO.Domain.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSO.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SSO.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SSO.Domain.Models.Application", b =>
                {
                    b.HasOne("SSO.Domain.Models.Realm", "Realm")
                        .WithMany("Applications")
                        .HasForeignKey("RealmId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Realm");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationCallback", b =>
                {
                    b.HasOne("SSO.Domain.Models.Application", "Application")
                        .WithMany("Callbacks")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationPermission", b =>
                {
                    b.HasOne("SSO.Domain.Models.Application", "Application")
                        .WithMany("Permissions")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationRole", b =>
                {
                    b.HasOne("SSO.Domain.Models.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationRoleClaim", b =>
                {
                    b.HasOne("SSO.Domain.Models.ApplicationPermission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SSO.Domain.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationUserClaim", b =>
                {
                    b.HasOne("SSO.Domain.Models.ApplicationPermission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSO.Domain.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");
                });

            modelBuilder.Entity("SSO.Domain.Models.Group", b =>
                {
                    b.HasOne("SSO.Domain.Models.Realm", "Realm")
                        .WithMany("Groups")
                        .HasForeignKey("RealmId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Realm");
                });

            modelBuilder.Entity("SSO.Domain.Models.GroupRole", b =>
                {
                    b.HasOne("SSO.Domain.Models.Group", "Group")
                        .WithMany("Roles")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSO.Domain.Models.ApplicationRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SSO.Domain.Models.GroupUser", b =>
                {
                    b.HasOne("SSO.Domain.Models.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SSO.Domain.Models.ApplicationUser", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSO.Domain.Models.RealmIdpSettings", b =>
                {
                    b.HasOne("SSO.Domain.Models.Realm", "Realm")
                        .WithMany("IdpSettingsCollection")
                        .HasForeignKey("RealmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Realm");
                });

            modelBuilder.Entity("SSO.Domain.Models.RealmUser", b =>
                {
                    b.HasOne("SSO.Domain.Models.Realm", "Realm")
                        .WithMany("Users")
                        .HasForeignKey("RealmId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SSO.Domain.Models.ApplicationUser", "User")
                        .WithMany("Realms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Realm");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSO.Domain.Models.RootUser", b =>
                {
                    b.HasOne("SSO.Domain.Models.ApplicationUser", "User")
                        .WithOne("RootUser")
                        .HasForeignKey("SSO.Domain.Models.RootUser", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SSO.Domain.Models.Application", b =>
                {
                    b.Navigation("Callbacks");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationUser", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Realms");

                    b.Navigation("RootUser")
                        .IsRequired();
                });

            modelBuilder.Entity("SSO.Domain.Models.Group", b =>
                {
                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("SSO.Domain.Models.Realm", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Groups");

                    b.Navigation("IdpSettingsCollection");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
