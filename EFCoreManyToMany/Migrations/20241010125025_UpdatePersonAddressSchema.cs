using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreManyToMany.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonAddressSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonAddressId",
                table: "PersonAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonAddressId",
                table: "PersonAddresses");
        }
    }
}
