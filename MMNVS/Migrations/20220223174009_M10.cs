using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_SettingProfiles_SelectedProfileId",
                table: "Settings");

            migrationBuilder.DropTable(
                name: "SettingProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Settings_SelectedProfileId",
                table: "Settings");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cd7afc5d-10ab-4a62-9167-331241d2cd96");

            migrationBuilder.DropColumn(
                name: "SelectedProfileId",
                table: "Settings");

            migrationBuilder.AddColumn<int>(
                name: "MinBatteryTimeForStart",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<int>(
                name: "TimeStartDuringShutdown",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UPSDataRefresh",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7a3919a1-faf2-4ede-872f-08ac81bfc6a7", 0, "e57876d9-3335-4ea2-9268-1da3361c3c45", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEDJT2T5wrTIZntN3Wq1wbMMDDs+e9MwgeWZuiV5vuJtqyFHDPYRnaJ0x3ER78HjAyA==", null, false, "a62fe44d-5f72-46c1-b60a-aa5056cb9e44", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7a3919a1-faf2-4ede-872f-08ac81bfc6a7");

            migrationBuilder.DropColumn(
                name: "MinBatteryTimeForStart",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TimeShutdownAfterPowerFailure",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TimeStartAfterPowerRecovery",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "TimeStartDuringShutdown",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "UPSDataRefresh",
                table: "Settings");

            migrationBuilder.AddColumn<int>(
                name: "SelectedProfileId",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SettingProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinBatteryTimeForStart = table.Column<int>(type: "int", nullable: false),
                    MinUPSCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeShutdownAfterPowerFailure = table.Column<int>(type: "int", nullable: false),
                    TimeStartAfterPowerRecovery = table.Column<int>(type: "int", nullable: false),
                    TimeStartDuringShutdown = table.Column<int>(type: "int", nullable: false),
                    UPSDataLog = table.Column<int>(type: "int", nullable: false),
                    UPSDataRefresh = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingProfiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cd7afc5d-10ab-4a62-9167-331241d2cd96", 0, "30132e09-8210-4fdc-9e07-b71b62db77f5", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEOJgkJ+MNrierpORBnCOp6RgA9ZbyHIX/OxVzyBSt/MJ81zv/K0XBpp4LC2jPJh6IA==", null, false, "40251e4e-217f-4cf1-bee9-0ae5536d6727", false, "administrator" });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_SelectedProfileId",
                table: "Settings",
                column: "SelectedProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_SettingProfiles_SelectedProfileId",
                table: "Settings",
                column: "SelectedProfileId",
                principalTable: "SettingProfiles",
                principalColumn: "Id");
        }
    }
}
