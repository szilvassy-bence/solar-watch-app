﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend.Data;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(SolarContext))]
    partial class SolarContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("backend.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "GB",
                            Latitude = 51.509864999999998,
                            Longitude = -0.118092,
                            Name = "London",
                            State = "England"
                        },
                        new
                        {
                            Id = 2,
                            Country = "HU",
                            Latitude = 47.497912999999997,
                            Longitude = 19.040236,
                            Name = "Budapest"
                        },
                        new
                        {
                            Id = 3,
                            Country = "FR",
                            Latitude = 48.864716000000001,
                            Longitude = 2.3490139999999999,
                            Name = "Paris",
                            State = "Ile-de-France"
                        });
                });

            modelBuilder.Entity("backend.Models.SunriseSunset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Sunrise")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Sunset")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("SunriseSunsets");
                });

            modelBuilder.Entity("backend.Models.SunriseSunset", b =>
                {
                    b.HasOne("backend.Models.City", "City")
                        .WithMany("SunriseSunsets")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("backend.Models.City", b =>
                {
                    b.Navigation("SunriseSunsets");
                });
#pragma warning restore 612, 618
        }
    }
}
