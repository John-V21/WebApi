﻿// <auto-generated />
using System;
using Accepted.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace accepted.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Accepted.Models.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("Date");

                    b.Property<TimeSpan>("MatchTime")
                        .HasColumnType("Time");

                    b.Property<decimal>("Sport")
                        .HasColumnType("Numeric(1)");

                    b.Property<string>("TeamA")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeamB")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("MatchDate");

                    b.HasIndex("TeamA");

                    b.HasIndex("TeamB");

                    b.HasIndex("TeamA", "TeamB");

                    b.HasIndex("TeamA", "TeamB", "MatchDate")
                        .IsUnique();

                    b.ToTable("Match");
                });

            modelBuilder.Entity("Accepted.Models.MatchOdd", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<decimal>("Odd")
                        .HasColumnType("decimal(3, 2)");

                    b.Property<string>("Specifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("MatchOdd");
                });

            modelBuilder.Entity("Accepted.Models.MatchOdd", b =>
                {
                    b.HasOne("Accepted.Models.Match", "Match")
                        .WithMany("MatchOdds")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
