using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matias_ToDo_DoubleDB.Migrations.DataDB
{
    /// <inheritdoc />
    public partial class Add_PublicKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "privateKey",
                table: "Cprs",
                newName: "PrivateKey");

            migrationBuilder.AddColumn<string>(
                name: "PublicKey",
                table: "Cprs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "Cprs");

            migrationBuilder.RenameColumn(
                name: "PrivateKey",
                table: "Cprs",
                newName: "privateKey");
        }
    }
}
