using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMNVS.Migrations
{
    public partial class M7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "administrator",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f8a5ce0e-f56f-4e67-90f3-be0543d55409", "ADMINISTRATOR", "AQAAAAEAACcQAAAAEPmrS7lQBie02OClpe/2ByZX01QTt6Bk9Dvnii2kZ6A0hFBr2AFZJCI1vjJ0sN0+OQ==", "831f94e5-1772-4584-987e-721e3bad28ef" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "administrator",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ae232d53-8686-40b0-8f16-2de5b205a831", null, "AQAAAAEAACcQAAAAEIHDjyxE+x5p3quLaxlYM46hzdi+w1DAuz3hMHYbQgQ4LXiEmwcTZi/c5tt7Wj9B5Q==", "3634ed64-cbeb-4e18-8736-537dba44561d" });
        }
    }
}
