using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R.Systems.Template.Persistence.Db.Migrations
{
    public partial class add_company_name_uniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_company_name",
                table: "company",
                column: "name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_company_name",
                table: "company");
        }
    }
}
