﻿// <auto-generated />
using GymPlanner.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymPlanner.Infrastructure.Migrations
{
    [DbContext(typeof(PlanDbContext))]
    [Migration("20240411065903_initialize")]
    partial class initialize
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GymPlanner.Domain.Entities.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "user"
                        });
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@mail.ru",
                            Password = "123456",
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plans.Excersise", b =>
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Упражнение 1"
                        });
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plans.Frequency", b =>
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Частота 1"
                        });
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plans.Plan", b =>
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "План 1",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plans.PlanExcersiseFrequency", b =>
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

                    b.HasData(
                        new
                        {
                            PlanId = 1,
                            FrequencyId = 1,
                            ExcersiseId = 1,
                            Id = 1,
                            Description = "15"
                        });
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Identity.User", b =>
                {
                    b.HasOne("GymPlanner.Domain.Entities.Identity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plans.Plan", b =>
                {
                    b.HasOne("GymPlanner.Domain.Entities.Identity.User", "User")
                        .WithMany("Plans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plans.PlanExcersiseFrequency", b =>
                {
                    b.HasOne("GymPlanner.Domain.Entities.Plans.Excersise", "Excersise")
                        .WithMany()
                        .HasForeignKey("ExcersiseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymPlanner.Domain.Entities.Plans.Frequency", "Frequency")
                        .WithMany()
                        .HasForeignKey("FrequencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymPlanner.Domain.Entities.Plans.Plan", "Plan")
                        .WithMany("planExcersiseFrequencies")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Excersise");

                    b.Navigation("Frequency");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Identity.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Identity.User", b =>
                {
                    b.Navigation("Plans");
                });

            modelBuilder.Entity("GymPlanner.Domain.Entities.Plans.Plan", b =>
                {
                    b.Navigation("planExcersiseFrequencies");
                });
#pragma warning restore 612, 618
        }
    }
}