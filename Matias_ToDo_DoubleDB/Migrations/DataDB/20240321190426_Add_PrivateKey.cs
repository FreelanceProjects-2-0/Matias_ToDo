using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matias_ToDo_DoubleDB.Migrations.DataDB
{
    /// <inheritdoc />
    public partial class Add_PrivateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "privateKey",
                table: "Cprs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "privateKey",
                table: "Cprs");
        }
    }
}
