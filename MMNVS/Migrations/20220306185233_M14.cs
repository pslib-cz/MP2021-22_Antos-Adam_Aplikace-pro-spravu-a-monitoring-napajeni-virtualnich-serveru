using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90f00bc0-ab24-4407-bd2c-13e133a20cb1");

            migrationBuilder.AddColumn<int>(
                name: "MinBatteryTimeForShutdown",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "vCenterVersion",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ed8a532c-c62d-4f11-a6a5-765976a85de0", 0, "541bf5a0-ce9d-4ea1-a883-a7c489a20512", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEEGDcQRI8H3JaAm5ZGG5Cq0wkPDeidr/s2jpfTtgfTGkKd+wzFABbs3aeg4OQHr4Pw==", null, false, "9d6f6eb7-e16e-47f7-a7f6-c580f30b26d5", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed8a532c-c62d-4f11-a6a5-765976a85de0");

            migrationBuilder.DropColumn(
                name: "MinBatteryTimeForShutdown",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "vCenterVersion",
                table: "Settings");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "90f00bc0-ab24-4407-bd2c-13e133a20cb1", 0, "a9c777ff-07e0-4c35-af05-42c4adb69da2", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEE9xs+hAtCH6AS+5iCiXqquJK8g62+5GoEC+LvQV11Tow5VDIW5XeSB7XF4oakT+Cw==", null, false, "86af79eb-9f8c-406e-8c8f-76ed0549f625", false, "administrator" });
        }
    }
}
