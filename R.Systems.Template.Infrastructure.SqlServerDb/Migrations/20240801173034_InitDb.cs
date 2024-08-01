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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    company_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    { new Guid("636786f1-e5aa-4a87-9c7d-e604a92f08f5"), "Google" },
                    { new Guid("9e27c3b4-bf21-4ffe-bdbb-919a2fc9e2cc"), "Meta" }
                });

            migrationBuilder.InsertData(
                table: "employee",
                columns: new[] { "id", "company_id", "first_name", "last_name" },
                values: new object[,]
                {
                    { new Guid("194ac2c8-72e3-4c63-8302-0217b9cc86b6"), new Guid("636786f1-e5aa-4a87-9c7d-e604a92f08f5"), "Jack", "Parker" },
                    { new Guid("424c6d87-3c97-4eb9-9a3e-0abbc7547683"), new Guid("9e27c3b4-bf21-4ffe-bdbb-919a2fc9e2cc"), "John", "Doe" },
                    { new Guid("878ae60f-c657-4465-8920-9d7d34f757ed"), new Guid("636786f1-e5aa-4a87-9c7d-e604a92f08f5"), "Will", "Smith" }
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
