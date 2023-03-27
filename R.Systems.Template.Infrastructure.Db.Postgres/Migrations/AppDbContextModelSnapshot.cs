﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace R.Systems.Template.Infrastructure.Db.Postgres.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("R.Systems.Template.Infrastructure.Db.Common.Entities.CompanyEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int?>("Id"), 3L, null, null, null, null, null);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("company", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Meta"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Google"
                        });
                });

            modelBuilder.Entity("R.Systems.Template.Infrastructure.Db.Common.Entities.EmployeeEntity", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int?>("Id"), 4L, null, null, null, null, null);

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer")
                        .HasColumnName("company_id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("employee", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CompanyId = 1,
                            FirstName = "John",
                            LastName = "Doe"
                        },
                        new
                        {
                            Id = 2,
                            CompanyId = 2,
                            FirstName = "Will",
                            LastName = "Smith"
                        },
                        new
                        {
                            Id = 3,
                            CompanyId = 2,
                            FirstName = "Jack",
                            LastName = "Parker"
                        });
                });

            modelBuilder.Entity("R.Systems.Template.Infrastructure.Db.Common.Entities.EmployeeEntity", b =>
                {
                    b.HasOne("R.Systems.Template.Infrastructure.Db.Common.Entities.CompanyEntity", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Company");
                });

            modelBuilder.Entity("R.Systems.Template.Infrastructure.Db.Common.Entities.CompanyEntity", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
