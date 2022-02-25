using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "administrator");

            migrationBuilder.AddColumn<bool>(
                name: "SmtpIsSecure",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cd7afc5d-10ab-4a62-9167-331241d2cd96", 0, "30132e09-8210-4fdc-9e07-b71b62db77f5", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEOJgkJ+MNrierpORBnCOp6RgA9ZbyHIX/OxVzyBSt/MJ81zv/K0XBpp4LC2jPJh6IA==", null, false, "40251e4e-217f-4cf1-bee9-0ae5536d6727", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cd7afc5d-10ab-4a62-9167-331241d2cd96");

            migrationBuilder.DropColumn(
                name: "SmtpIsSecure",
                table: "Settings");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "administrator", 0, "f8082a61-c4d1-4050-94a1-a5b558b6027c", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEIEHxr8Usdsg37odcIl/ajCfubVkRHwfnjGo5twAr0DOTDrQSHzTxXckjJLXRIRr2A==", null, false, "12d33982-993d-4084-9e56-816163c4a04e", false, "administrator" });
        }
    }
}
