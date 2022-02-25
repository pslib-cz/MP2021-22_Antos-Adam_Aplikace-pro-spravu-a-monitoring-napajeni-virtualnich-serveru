using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8670a737-45b5-4400-baec-e66b0992f0c2");

            migrationBuilder.AddColumn<int>(
                name: "DelayTimeVMStart",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SystemState",
                table: "Log",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "90f00bc0-ab24-4407-bd2c-13e133a20cb1", 0, "a9c777ff-07e0-4c35-af05-42c4adb69da2", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEE9xs+hAtCH6AS+5iCiXqquJK8g62+5GoEC+LvQV11Tow5VDIW5XeSB7XF4oakT+Cw==", null, false, "86af79eb-9f8c-406e-8c8f-76ed0549f625", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90f00bc0-ab24-4407-bd2c-13e133a20cb1");

            migrationBuilder.DropColumn(
                name: "DelayTimeVMStart",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SystemState",
                table: "Log");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8670a737-45b5-4400-baec-e66b0992f0c2", 0, "0839988d-d4ab-41dd-98a8-1a3b0ad05a10", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEOT2KwDvHqQ+9Fi/1QmM1xFenO7v9LL9NXUyIBmoG1L1sbrwUV70+6ar72P8tS+2eQ==", null, false, "a2009eb7-dd76-487d-ba3c-cb8b4dae1eeb", false, "administrator" });
        }
    }
}
