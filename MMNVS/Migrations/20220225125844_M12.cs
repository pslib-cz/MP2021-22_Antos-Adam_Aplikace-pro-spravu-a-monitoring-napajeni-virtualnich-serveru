using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "de23b6e7-292d-41f4-ad10-c9f012dcd158");

            migrationBuilder.DropColumn(
                name: "TimeShutdownAfterPowerFailure",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TimeStartAfterPowerRecovery",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "UPSDataRefresh",
                table: "Settings",
                newName: "DelayTimeHosts");

            migrationBuilder.RenameColumn(
                name: "TimeStartDuringShutdown",
                table: "Settings",
                newName: "DelayTimeDatastores");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8670a737-45b5-4400-baec-e66b0992f0c2", 0, "0839988d-d4ab-41dd-98a8-1a3b0ad05a10", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEOT2KwDvHqQ+9Fi/1QmM1xFenO7v9LL9NXUyIBmoG1L1sbrwUV70+6ar72P8tS+2eQ==", null, false, "a2009eb7-dd76-487d-ba3c-cb8b4dae1eeb", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8670a737-45b5-4400-baec-e66b0992f0c2");

            migrationBuilder.RenameColumn(
                name: "DelayTimeHosts",
                table: "Settings",
                newName: "UPSDataRefresh");

            migrationBuilder.RenameColumn(
                name: "DelayTimeDatastores",
                table: "Settings",
                newName: "TimeStartDuringShutdown");

            migrationBuilder.AddColumn<int>(
                name: "TimeShutdownAfterPowerFailure",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeStartAfterPowerRecovery",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "de23b6e7-292d-41f4-ad10-c9f012dcd158", 0, "a9757f22-e09b-44d6-a5f7-7619c8f4f194", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEPBNPMjdHXTfma89Bd1n/Mp0Ti6XxWpgSKJNAhyEMvMXsDis/GC5xB1J4jFQXqpaZA==", null, false, "ae4cc3ac-7d22-4579-b90b-27c9681fb998", false, "administrator" });
        }
    }
}
