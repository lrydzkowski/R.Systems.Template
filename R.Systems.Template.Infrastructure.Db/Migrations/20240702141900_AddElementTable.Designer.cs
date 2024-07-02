﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using R.Systems.Template.Infrastructure.Db;

#nullable disable

namespace R.Systems.Template.Infrastructure.Db.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240702141900_AddElementTable")]
    partial class AddElementTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("R.Systems.Template.Infrastructure.Db.Common.Entities.CompanyEntity", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long?>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long?>("Id"), 3L, null, null, null, null, null);

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
                            Id = 1L,
                            Name = "Meta"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Google"
                        });
                });

            modelBuilder.Entity("R.Systems.Template.Infrastructure.Db.Common.Entities.ElementEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int?>("AdditionalValue")
                        .HasColumnType("integer")
                        .HasColumnName("additional_value");

                    b.Property<long?>("BigAdditionalValue")
                        .HasColumnType("bigint")
                        .HasColumnName("big_additional_value");

                    b.Property<long>("BigValue")
                        .HasColumnType("bigint")
                        .HasColumnName("big_value");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("date")
                        .HasColumnName("creation_date");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date_time");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<decimal?>("Discount")
                        .HasColumnType("numeric")
                        .HasColumnName("discount");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsNew")
                        .HasColumnType("boolean")
                        .HasColumnName("is_new");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("date")
                        .HasColumnName("update_date");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("update_date_time");

                    b.Property<int>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("element", (string)null);
                });

            modelBuilder.Entity("R.Systems.Template.Infrastructure.Db.Common.Entities.EmployeeEntity", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long?>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long?>("Id"), 4L, null, null, null, null, null);

                    b.Property<long?>("CompanyId")
                        .HasColumnType("bigint")
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
                            Id = 1L,
                            CompanyId = 1L,
                            FirstName = "John",
                            LastName = "Doe"
                        },
                        new
                        {
                            Id = 2L,
                            CompanyId = 2L,
                            FirstName = "Will",
                            LastName = "Smith"
                        },
                        new
                        {
                            Id = 3L,
                            CompanyId = 2L,
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
