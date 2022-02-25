using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VirtualServers_HostServers_PreferedHostId",
                table: "VirtualServers");

            migrationBuilder.DropIndex(
                name: "IX_VirtualServers_PreferedHostId",
                table: "VirtualServers");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7a3919a1-faf2-4ede-872f-08ac81bfc6a7");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "VirtualStorageServers");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "VirtualServers");

            migrationBuilder.DropColumn(
                name: "PreferedHostId",
                table: "VirtualServers");

            migrationBuilder.AddColumn<bool>(
                name: "IsvCenter",
                table: "VirtualServers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "de23b6e7-292d-41f4-ad10-c9f012dcd158", 0, "a9757f22-e09b-44d6-a5f7-7619c8f4f194", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEPBNPMjdHXTfma89Bd1n/Mp0Ti6XxWpgSKJNAhyEMvMXsDis/GC5xB1J4jFQXqpaZA==", null, false, "ae4cc3ac-7d22-4579-b90b-27c9681fb998", false, "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "de23b6e7-292d-41f4-ad10-c9f012dcd158");

            migrationBuilder.DropColumn(
                name: "IsvCenter",
                table: "VirtualServers");

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "VirtualStorageServers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "VirtualServers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreferedHostId",
                table: "VirtualServers",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Notes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7a3919a1-faf2-4ede-872f-08ac81bfc6a7", 0, "e57876d9-3335-4ea2-9268-1da3361c3c45", null, false, false, null, null, "ADMINISTRATOR", null, "AQAAAAEAACcQAAAAEDJT2T5wrTIZntN3Wq1wbMMDDs+e9MwgeWZuiV5vuJtqyFHDPYRnaJ0x3ER78HjAyA==", null, false, "a62fe44d-5f72-46c1-b60a-aa5056cb9e44", false, "administrator" });

            migrationBuilder.CreateIndex(
                name: "IX_VirtualServers_PreferedHostId",
                table: "VirtualServers",
                column: "PreferedHostId");

            migrationBuilder.AddForeignKey(
                name: "FK_VirtualServers_HostServers_PreferedHostId",
                table: "VirtualServers",
                column: "PreferedHostId",
                principalTable: "HostServers",
                principalColumn: "Id");
        }
    }
}
