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
                keyValue: "ca67d5d3-08f5-4a02-97c3-67a548c379fc");

            migrationBuilder.AddColumn<bool>(
                name: "IsOSWindows",
                table: "HostServers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ede8d844-6375-4d0e-8ca7-b9b006a739e0", 0, "51ea11b8-d6b8-45de-ab86-28c08820a7e0", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEOyCOmVV6fk0p7Z/rPZnDVMgFe5+MUEbrRMX66SUp9q8oqe9sEPuPNQzcFt2XN1nog==", null, false, "a5a49f83-cb28-417d-927f-d71215751c49", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ede8d844-6375-4d0e-8ca7-b9b006a739e0");

            migrationBuilder.DropColumn(
                name: "IsOSWindows",
                table: "HostServers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ca67d5d3-08f5-4a02-97c3-67a548c379fc", 0, "67fd8e68-0f4d-4848-84ae-d0a78e494f4c", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEJ5rDtbb2twsqh5sXciBvJECGLhFBpOrCbDfw5qtZVP5oM+/6NT3PlvcYB8QLGMWDw==", null, false, "ec4dca56-bac3-4d9f-a2bd-efdfc82f6ef0", false, "administrator" });
        }
    }
}
