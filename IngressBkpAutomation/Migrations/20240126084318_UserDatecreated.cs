using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngressBkpAutomation.Migrations
{
    /// <inheritdoc />
    public partial class UserDatecreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "datetime",
                nullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "AutoBackup2At",
                table: "SysSetup",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "AutoBackup1At",
                table: "SysSetup",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Users");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "AutoBackup2At",
                table: "SysSetup",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "AutoBackup1At",
                table: "SysSetup",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }
    }
}
