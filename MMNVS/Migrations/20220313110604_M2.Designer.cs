﻿// <auto-generated />
using System;
using MMNVS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MMNVS.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220313110604_M2")]
    partial class M2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MMNVS.Model.AppSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AdministratorEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DelayTime")
                        .HasColumnType("int");

                    b.Property<int>("DelayTimeDatastores")
                        .HasColumnType("int");

                    b.Property<int>("DelayTimeHosts")
                        .HasColumnType("int");

                    b.Property<int>("DelayTimeVMStart")
                        .HasColumnType("int");

                    b.Property<int>("MinBatteryTimeForShutdown")
                        .HasColumnType("int");

                    b.Property<int>("MinBatteryTimeForStart")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PrimaryUPSId")
                        .HasColumnType("int");

                    b.Property<bool>("SmtpIsSecure")
                        .HasColumnType("bit");

                    b.Property<string>("SmtpPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SmtpPort")
                        .HasColumnType("int");

                    b.Property<string>("SmtpServer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SmtpUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SystemState")
                        .HasColumnType("int");

                    b.Property<string>("vCenterApiUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vCenterIP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vCenterPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vCenterUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("vCenterVersion")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryUPSId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("MMNVS.Model.Datastore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VirtualStorageServerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VirtualStorageServerId");

                    b.ToTable("Datastores");
                });

            modelBuilder.Entity("MMNVS.Model.HostServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ESXiIPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ESXiPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ESXiUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Producer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("iLoIPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("iLoPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("iLoUser")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HostServers");
                });

            modelBuilder.Entity("MMNVS.Model.LogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("DatastoreId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("HostServerId")
                        .HasColumnType("int");

                    b.Property<int>("OperationType")
                        .HasColumnType("int");

                    b.Property<int>("SystemState")
                        .HasColumnType("int");

                    b.Property<int?>("UPSId")
                        .HasColumnType("int");

                    b.Property<string>("VirtualServerVMId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("VirtualStorageServerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DatastoreId");

                    b.HasIndex("HostServerId");

                    b.HasIndex("UPSId");

                    b.HasIndex("VirtualServerVMId");

                    b.HasIndex("VirtualStorageServerId");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("MMNVS.Model.MyUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "eb6ff04e-1704-4d42-9b11-1f9ffbc403ea",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "4f4c87b5-2934-4ed1-96a1-8162e0714138",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMINISTRATOR",
                            PasswordHash = "AQAAAAEAACcQAAAAEJpppji8nrZgTtnfPO3/cKI/X/nv1lcMdiEM6tc+GWD4J/QdP3DB/RA7ncKjPb9/kw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "0d571556-0c53-424a-9366-32101a7e21b4",
                            TwoFactorEnabled = false,
                            UserName = "administrator"
                        });
                });

            modelBuilder.Entity("MMNVS.Model.UPS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Producer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SNMPOIDBatteryCapacity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SNMPOIDBatteryTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SNMPOIDLoad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SNMPOIDState")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UPS");
                });

            modelBuilder.Entity("MMNVS.Model.UPSLogItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BatteryCapacity")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Error")
                        .HasColumnType("bit");

                    b.Property<int>("Load")
                        .HasColumnType("int");

                    b.Property<int>("RemainingTime")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int?>("UPSId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UPSId");

                    b.ToTable("UPSLog");
                });

            modelBuilder.Entity("MMNVS.Model.VirtualServer", b =>
                {
                    b.Property<string>("VMId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsvCenter")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<bool>("StartServerOnStart")
                        .HasColumnType("bit");

                    b.HasKey("VMId");

                    b.ToTable("VirtualServers");
                });

            modelBuilder.Entity("MMNVS.Model.VirtualStorageServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("HostId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("VirtualStorageServers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MMNVS.Model.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MMNVS.Model.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MMNVS.Model.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MMNVS.Model.MyUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MMNVS.Model.AppSettings", b =>
                {
                    b.HasOne("MMNVS.Model.UPS", "PrimaryUPS")
                        .WithMany("AppSettings")
                        .HasForeignKey("PrimaryUPSId");

                    b.Navigation("PrimaryUPS");
                });

            modelBuilder.Entity("MMNVS.Model.Datastore", b =>
                {
                    b.HasOne("MMNVS.Model.VirtualStorageServer", "VirtualStorageServer")
                        .WithMany("Datastores")
                        .HasForeignKey("VirtualStorageServerId");

                    b.Navigation("VirtualStorageServer");
                });

            modelBuilder.Entity("MMNVS.Model.LogItem", b =>
                {
                    b.HasOne("MMNVS.Model.Datastore", "Datastore")
                        .WithMany("Log")
                        .HasForeignKey("DatastoreId");

                    b.HasOne("MMNVS.Model.HostServer", "HostServer")
                        .WithMany("Log")
                        .HasForeignKey("HostServerId");

                    b.HasOne("MMNVS.Model.UPS", "UPS")
                        .WithMany("Log")
                        .HasForeignKey("UPSId");

                    b.HasOne("MMNVS.Model.VirtualServer", "VirtualServer")
                        .WithMany("Log")
                        .HasForeignKey("VirtualServerVMId");

                    b.HasOne("MMNVS.Model.VirtualStorageServer", "VirtualStorageServer")
                        .WithMany("Log")
                        .HasForeignKey("VirtualStorageServerId");

                    b.Navigation("Datastore");

                    b.Navigation("HostServer");

                    b.Navigation("UPS");

                    b.Navigation("VirtualServer");

                    b.Navigation("VirtualStorageServer");
                });

            modelBuilder.Entity("MMNVS.Model.UPSLogItem", b =>
                {
                    b.HasOne("MMNVS.Model.UPS", "UPS")
                        .WithMany("UPSLog")
                        .HasForeignKey("UPSId");

                    b.Navigation("UPS");
                });

            modelBuilder.Entity("MMNVS.Model.VirtualStorageServer", b =>
                {
                    b.HasOne("MMNVS.Model.HostServer", "Host")
                        .WithMany("VirtualStorageServers")
                        .HasForeignKey("HostId");

                    b.Navigation("Host");
                });

            modelBuilder.Entity("MMNVS.Model.Datastore", b =>
                {
                    b.Navigation("Log");
                });

            modelBuilder.Entity("MMNVS.Model.HostServer", b =>
                {
                    b.Navigation("Log");

                    b.Navigation("VirtualStorageServers");
                });

            modelBuilder.Entity("MMNVS.Model.UPS", b =>
                {
                    b.Navigation("AppSettings");

                    b.Navigation("Log");

                    b.Navigation("UPSLog");
                });

            modelBuilder.Entity("MMNVS.Model.VirtualServer", b =>
                {
                    b.Navigation("Log");
                });

            modelBuilder.Entity("MMNVS.Model.VirtualStorageServer", b =>
                {
                    b.Navigation("Datastores");

                    b.Navigation("Log");
                });
#pragma warning restore 612, 618
        }
    }
}
