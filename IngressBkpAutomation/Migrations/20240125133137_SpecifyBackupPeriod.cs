using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngressBkpAutomation.Migrations
{
    /// <inheritdoc />
    public partial class SpecifyBackupPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "AutoBackup1At",
                table: "SysSetup",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AutoBackup2At",
                table: "SysSetup",
                type: "time(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoBackup1At",
                table: "SysSetup");

            migrationBuilder.DropColumn(
                name: "AutoBackup2At",
                table: "SysSetup");
        }
    }
}
