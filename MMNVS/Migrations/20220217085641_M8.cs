using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "SystemState",
                table: "Settings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "administrator",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f8082a61-c4d1-4050-94a1-a5b558b6027c", "AQAAAAEAACcQAAAAEIEHxr8Usdsg37odcIl/ajCfubVkRHwfnjGo5twAr0DOTDrQSHzTxXckjJLXRIRr2A==", "12d33982-993d-4084-9e56-816163c4a04e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemState",
                table: "Settings");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "administrator",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f8a5ce0e-f56f-4e67-90f3-be0543d55409", "AQAAAAEAACcQAAAAEPmrS7lQBie02OClpe/2ByZX01QTt6Bk9Dvnii2kZ6A0hFBr2AFZJCI1vjJ0sN0+OQ==", "831f94e5-1772-4584-987e-721e3bad28ef" });
        }
    }
}
