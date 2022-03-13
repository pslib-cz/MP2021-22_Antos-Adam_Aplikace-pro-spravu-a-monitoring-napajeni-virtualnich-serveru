using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "eb6ff04e-1704-4d42-9b11-1f9ffbc403ea");

            migrationBuilder.AddColumn<int>(
                name: "BatteryTimeForShutdownHosts",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "84e2d884-181f-41b5-8137-c47b8a577011", 0, "df2beb6e-0d39-4acf-b6aa-43ff3dde1cf4", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEJiQ3nP74GFBu9dHClqpiGe+B14Z3k5pJZeuu/LiDmaL28W6FiWoRLuRHPpdyOdoZQ==", null, false, "88961103-d410-4d53-9c20-9d1f2563bbad", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "84e2d884-181f-41b5-8137-c47b8a577011");

            migrationBuilder.DropColumn(
                name: "BatteryTimeForShutdownHosts",
                table: "Settings");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "eb6ff04e-1704-4d42-9b11-1f9ffbc403ea", 0, "4f4c87b5-2934-4ed1-96a1-8162e0714138", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEJpppji8nrZgTtnfPO3/cKI/X/nv1lcMdiEM6tc+GWD4J/QdP3DB/RA7ncKjPb9/kw==", null, false, "0d571556-0c53-424a-9366-32101a7e21b4", false, "administrator" });
        }
    }
}
