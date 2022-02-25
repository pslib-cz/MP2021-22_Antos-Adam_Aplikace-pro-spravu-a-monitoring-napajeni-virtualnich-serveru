using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_SettingProfiles_SelectedProfileId",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_UPS_PrimaryUPSId",
                table: "Settings");

            migrationBuilder.AlterColumn<int>(
                name: "SmtpPort",
                table: "Settings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SelectedProfileId",
                table: "Settings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PrimaryUPSId",
                table: "Settings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_SettingProfiles_SelectedProfileId",
                table: "Settings",
                column: "SelectedProfileId",
                principalTable: "SettingProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_UPS_PrimaryUPSId",
                table: "Settings",
                column: "PrimaryUPSId",
                principalTable: "UPS",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_SettingProfiles_SelectedProfileId",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_UPS_PrimaryUPSId",
                table: "Settings");

            migrationBuilder.AlterColumn<int>(
                name: "SmtpPort",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SelectedProfileId",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrimaryUPSId",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_SettingProfiles_SelectedProfileId",
                table: "Settings",
                column: "SelectedProfileId",
                principalTable: "SettingProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_UPS_PrimaryUPSId",
                table: "Settings",
                column: "PrimaryUPSId",
                principalTable: "UPS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
