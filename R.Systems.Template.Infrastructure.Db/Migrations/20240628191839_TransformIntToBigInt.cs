using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace R.Systems.Template.Infrastructure.Db.Migrations
{
    /// <inheritdoc />
    public partial class TransformIntToBigInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "employee",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "employee",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "employee",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "company",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "company",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.AlterColumn<long>(
                name: "company_id",
                table: "employee",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "employee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'4', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'4', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "company",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "employee",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "employee",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "employee",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "company",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "company",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.AlterColumn<int>(
                name: "company_id",
                table: "employee",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "employee",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:IdentitySequenceOptions", "'4', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'4', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "company",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "company",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Meta" },
                    { 2, "Google" }
                });

            migrationBuilder.InsertData(
                table: "employee",
                columns: new[] { "id", "company_id", "first_name", "last_name" },
                values: new object[,]
                {
                    { 1, 1, "John", "Doe" },
                    { 2, 2, "Will", "Smith" },
                    { 3, 2, "Jack", "Parker" }
                });
        }
    }
}
