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
                keyValue: "3698bea7-d05c-4edb-8cc8-0241b3160724");

            migrationBuilder.AddColumn<bool>(
                name: "TestMode",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ca67d5d3-08f5-4a02-97c3-67a548c379fc", 0, "67fd8e68-0f4d-4848-84ae-d0a78e494f4c", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEJ5rDtbb2twsqh5sXciBvJECGLhFBpOrCbDfw5qtZVP5oM+/6NT3PlvcYB8QLGMWDw==", null, false, "ec4dca56-bac3-4d9f-a2bd-efdfc82f6ef0", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ca67d5d3-08f5-4a02-97c3-67a548c379fc");

            migrationBuilder.DropColumn(
                name: "TestMode",
                table: "Settings");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3698bea7-d05c-4edb-8cc8-0241b3160724", 0, "806847c7-6d75-4ec0-9bd3-a27227c273f5", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAENOrrahD5qEbMBy3teBM3nc7bsptYAR/Ii8aqSbid/S2Z97KrXqOfSvmzBvg8aE6Jg==", null, false, "ec86b9ae-41a7-4596-a975-e668a11970a1", false, "administrator" });
        }
    }
}
