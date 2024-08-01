using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace R.Systems.Template.Infrastructure.PostgreSqlDb.Migrations
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "element",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    value = table.Column<int>(type: "integer", nullable: false),
                    additional_value = table.Column<int>(type: "integer", nullable: true),
                    big_value = table.Column<long>(type: "bigint", nullable: false),
                    big_additional_value = table.Column<long>(type: "bigint", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    discount = table.Column<decimal>(type: "numeric", nullable: true),
                    creation_date = table.Column<DateOnly>(type: "date", nullable: false),
                    update_date = table.Column<DateOnly>(type: "date", nullable: true),
                    creation_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_new = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_element", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: true)
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
                    { new Guid("31b04626-ed12-4d79-b3d6-1430a72000d5"), "Meta" },
                    { new Guid("9427a96c-a0b6-461c-814c-9c3c2bb6ff80"), "Google" }
                });

            migrationBuilder.InsertData(
                table: "employee",
                columns: new[] { "id", "company_id", "first_name", "last_name" },
                values: new object[,]
                {
                    { new Guid("84b096f7-68a1-47a8-9e6a-8cfd79f0f069"), new Guid("31b04626-ed12-4d79-b3d6-1430a72000d5"), "John", "Doe" },
                    { new Guid("ab189e89-7007-43bf-85d1-b1cc3c69c503"), new Guid("9427a96c-a0b6-461c-814c-9c3c2bb6ff80"), "Will", "Smith" },
                    { new Guid("b82f922b-784c-40e1-b03b-476a0b447dca"), new Guid("9427a96c-a0b6-461c-814c-9c3c2bb6ff80"), "Jack", "Parker" }
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
