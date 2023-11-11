using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    public partial class SeedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            for (int i = 1; i <= 50; i++)
            {
                migrationBuilder.InsertData(
                    "Users",
                        columns: new[]
                        {
                            "Id",
                            "UserName",
                            "Email",
                            "EmailConfirmed",
                             "SecurityStamp",
                            "ConcurrencyStamp",
                            "PhoneNumber",
                            "PhoneNumberConfirmed",
                            "TwoFactorEnabled",
                            "LockoutEnabled",
                            "AccessFailedCount",
                            "Address"

                        },
                        values: new object[]
                        {
                            Guid.NewGuid().ToString(),
                            "User"+i.ToString(),
                            $"email{i.ToString()}@example.com",
                            true,
                            Guid.NewGuid().ToString(),
                            Guid.NewGuid().ToString(),
                            "0123456789",
                            true,
                            false,
                            false,
                            0,
                            "Home Address"
                        });
            }
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
