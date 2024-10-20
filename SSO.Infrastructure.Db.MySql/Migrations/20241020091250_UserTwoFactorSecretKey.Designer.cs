﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSO.Infrastructure.Db.MySql;

#nullable disable

namespace SSO.Infrastructure.Db.MySql.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241020091250_UserTwoFactorSecretKey")]
    partial class UserTwoFactorSecretKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.Application", b =>
                {
                    b.Property<Guid>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTimeOffset>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset?>("DateInactive")
                        .HasColumnType("datetime");

                    b.Property<DateTimeOffset>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<short>("MaxAccessFailedCount")
                        .HasColumnType("smallint");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<Guid>("RealmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValue(new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));

                    b.Property<int>("RefreshTokenExpiration")
                        .HasColumnType("int");

                    b.Property<int>("TokenExpiration")
                        .HasColumnType("int");

                    b.HasKey("ApplicationId");

                    b.HasIndex("RealmId", "Name")
                        .IsUnique();

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationCallback", b =>
                {
                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Url")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("ApplicationId", "Url");

                    b.ToTable("ApplicationCallbacks");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationPermission", b =>
                {
                    b.Property<Guid>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("PermissionId");

                    b.HasIndex("ApplicationId", "Name")
                        .IsUnique();

                    b.ToTable("ApplicationPermissions");
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

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
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasDefaultValue("admin");

                    b.Property<DateTime?>("DateConfirmed")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DateInactive")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasDefaultValue("admin");

                    b.Property<DateTime?>("LastFailedPasswordAttempt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasDefaultValue("admin");

                    b.Property<DateTime?>("LastPasswordChanged")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastSessionId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasDefaultValue("35c7c988-7c48-4f13-bf41-4edbd060a394");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasDefaultValue("admin");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<DateTime?>("PasswordExpiry")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("TwoFactorSecretKey")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique();

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("SSO.Domain.Models.Group", b =>
                {
                    b.Property<Guid>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DateInactive")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<Guid>("RealmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasDefaultValue(new Guid("afb6d3ad-92a3-4ea3-aa90-88071a3ee8aa"));

                    b.HasKey("GroupId");

                    b.HasIndex("RealmId", "Name")
                        .IsUnique();

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SSO.Domain.Models.GroupRole", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("GroupId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("GroupRoles");
                });

            modelBuilder.Entity("SSO.Domain.Models.GroupUser", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("char(36)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("SSO.Domain.Models.Realm", b =>
                {
                    b.Property<Guid>("RealmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("DateInactive")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("RealmId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Realms");
                });

            modelBuilder.Entity("SSO.Domain.Models.RealmIdpSettings", b =>
                {
                    b.Property<Guid>("RealmId")
                        .HasColumnType("char(36)");

                    b.Property<sbyte>("IdentityProvider")
                        .HasColumnType("tinyint");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("RealmId", "IdentityProvider");

                    b.ToTable("RealmIdpSettings");
                });

            modelBuilder.Entity("SSO.Domain.Models.RealmUser", b =>
                {
                    b.Property<Guid>("RealmId")
                        .HasColumnType("char(36)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("RealmId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("RealmUsers");
                });

            modelBuilder.Entity("SSO.Domain.Models.RootUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

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
