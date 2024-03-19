using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matias_ToDo_DoubleDB.Migrations.DataDB
{
    /// <inheritdoc />
    public partial class Add_Cpr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cprs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CprNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserMail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cprs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cprs");
        }
    }
}
