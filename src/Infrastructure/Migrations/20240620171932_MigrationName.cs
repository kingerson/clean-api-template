#nullable disable

namespace MsClean.Infrastructure.Migrations;
using System;
using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class MigrationName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Person",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false),
                Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                LastName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                UserRegister = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                UserUpdated = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                DateTimeUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                DateTimeRegister = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Person", x => x.Id);
            })
            .Annotation("MySQL:Charset", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Person");
    }
}
