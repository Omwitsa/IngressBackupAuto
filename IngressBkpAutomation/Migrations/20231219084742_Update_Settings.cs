using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngressBkpAutomation.Migrations
{
    /// <inheritdoc />
    public partial class Update_Settings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HoMysqlPassword",
                table: "SysSetup",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HoMysqlServer",
                table: "SysSetup",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HoMysqlUserName",
                table: "SysSetup",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "OnMpls",
                table: "SysSetup",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoMysqlPassword",
                table: "SysSetup");

            migrationBuilder.DropColumn(
                name: "HoMysqlServer",
                table: "SysSetup");

            migrationBuilder.DropColumn(
                name: "HoMysqlUserName",
                table: "SysSetup");

            migrationBuilder.DropColumn(
                name: "OnMpls",
                table: "SysSetup");
        }
    }
}
