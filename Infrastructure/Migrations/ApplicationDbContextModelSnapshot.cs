﻿// <auto-generated />
using GymPlanner.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymPlanner.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.Excersise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.HasKey("Id");

                    b.ToTable("Excersises");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.Frequency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Frequencies");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.PlanExcersiseFrequency", b =>
                {
                    b.Property<int>("PlanId")
                        .HasColumnType("int");

                    b.Property<int>("FrequencyId")
                        .HasColumnType("int");

                    b.Property<int>("ExcersiseId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("PlanId", "FrequencyId", "ExcersiseId", "Id");

                    b.HasIndex("ExcersiseId");

                    b.HasIndex("FrequencyId");

                    b.ToTable("PlanExcersiseFrequencys");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Alex",
                            LastName = "Xela"
                        });
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.Plan", b =>
                {
                    b.HasOne("GymPlanner.Domain.Entities.Plan.User", "User")
                        .WithMany("Plans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.PlanExcersiseFrequency", b =>
                {
                    b.HasOne("GymPlanner.Domain.Entities.Plan.Excersise", "Excersise")
                        .WithMany()
                        .HasForeignKey("ExcersiseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymPlanner.Domain.Entities.Plan.Frequency", "Frequency")
                        .WithMany()
                        .HasForeignKey("FrequencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymPlanner.Domain.Entities.Plan.Plan", "Plan")
                        .WithMany("planExcersiseFrequencies")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Excersise");

                    b.Navigation("Frequency");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.Plan", b =>
                {
                    b.Navigation("planExcersiseFrequencies");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plan.User", b =>
                {
                    b.Navigation("Plans");
                });
#pragma warning restore 612, 618
        }
    }
}