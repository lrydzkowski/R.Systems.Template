using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace R.Systems.Template.Infrastructure.SqlServerDb.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "3, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "element",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    value = table.Column<int>(type: "int", nullable: false),
                    additional_value = table.Column<int>(type: "int", nullable: true),
                    big_value = table.Column<long>(type: "bigint", nullable: false),
                    big_additional_value = table.Column<long>(type: "bigint", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    creation_date = table.Column<DateOnly>(type: "date", nullable: false),
                    update_date = table.Column<DateOnly>(type: "date", nullable: true),
                    creation_date_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_date_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_new = table.Column<bool>(type: "bit", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_element", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "4, 1"),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    company_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "company",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1L, "Meta" },
                    { 2L, "Google" }
                });

            migrationBuilder.InsertData(
                table: "employee",
                columns: new[] { "id", "company_id", "first_name", "last_name" },
                values: new object[,]
                {
                    { 1L, 1L, "John", "Doe" },
                    { 2L, 2L, "Will", "Smith" },
                    { 3L, 2L, "Jack", "Parker" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_company_name",
                table: "company",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_company_id",
                table: "employee",
                column: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "element");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "company");
        }
    }
}
