using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5b8083ee-6664-4165-b4e4-26aea09ee586");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "VirtualStorageServers");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Datastores");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "eb6ff04e-1704-4d42-9b11-1f9ffbc403ea", 0, "4f4c87b5-2934-4ed1-96a1-8162e0714138", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEJpppji8nrZgTtnfPO3/cKI/X/nv1lcMdiEM6tc+GWD4J/QdP3DB/RA7ncKjPb9/kw==", null, false, "0d571556-0c53-424a-9366-32101a7e21b4", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "eb6ff04e-1704-4d42-9b11-1f9ffbc403ea");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "VirtualStorageServers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Datastores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5b8083ee-6664-4165-b4e4-26aea09ee586", 0, "ffb7a982-9b02-4750-9ac5-0deae53dd60d", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEFJEpingfwaQpDs8FnKWRd6f69x8griLbDAxXCA4KZfwp+xsSIIns2QYTGGEMhdJ9A==", null, false, "7407ba99-a2fa-4246-8b6d-c0daf52ff7e0", false, "administrator" });
        }
    }
}
