﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiMotoServer.Data;

#nullable disable

namespace ServiMotoServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240526164237_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("ServiMotoServer.Models.Motorcycle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MotorcycleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Motorcycles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("45aa7ade-dd9f-47c4-a768-93af5d4ab2c3"),
                            Description = "Motorcycle 1",
                            MotorcycleName = "motorcycle1",
                            Password = "moto123"
                        },
                        new
                        {
                            Id = new Guid("e7408fde-2c8f-455e-8bdb-672ba5ee537c"),
                            Description = "Motorcycle 2",
                            MotorcycleName = "motorcycle2",
                            Password = "moto123"
                        },
                        new
                        {
                            Id = new Guid("fc876abb-9bef-4609-a6c7-5efa520d64a1"),
                            Description = "Motorcycle 3",
                            MotorcycleName = "motorcycle3",
                            Password = "moto123"
                        });
                });

            modelBuilder.Entity("ServiMotoServer.Models.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ServiceName")
                        .IsUnique();

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8ffd5261-a6e7-4c56-a21e-3346b42294b4"),
                            Description = "Description for Service 1",
                            ServiceName = "Service1"
                        },
                        new
                        {
                            Id = new Guid("b958aadd-506a-4fb7-acac-ec88503ee279"),
                            Description = "Description for Service 2",
                            ServiceName = "Service2"
                        },
                        new
                        {
                            Id = new Guid("8da8b3df-0123-446f-ad12-43eff057fa55"),
                            Description = "Description for Service 3",
                            ServiceName = "Service3"
                        });
                });

            modelBuilder.Entity("ServiMotoServer.Models.ServiceAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MotorcycleId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MotorcycleId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("UserId");

                    b.ToTable("ServiceAssignments");

                    b.HasData(
                        new
                        {
                            Id = new Guid("888f586d-5247-406d-9b9d-13e5cf088c7f"),
                            ServiceId = new Guid("8ffd5261-a6e7-4c56-a21e-3346b42294b4"),
                            UserId = new Guid("b1ffb1f6-71f8-42d2-8c54-31d3aff66863")
                        },
                        new
                        {
                            Id = new Guid("1f927b84-4ba6-43a5-b515-5d5b28cb7808"),
                            ServiceId = new Guid("b958aadd-506a-4fb7-acac-ec88503ee279"),
                            UserId = new Guid("f2bd003f-d22d-4080-ab46-09db6b5e4ee7")
                        },
                        new
                        {
                            Id = new Guid("2aafec2b-9f37-410a-bfb4-185472997106"),
                            ServiceId = new Guid("8da8b3df-0123-446f-ad12-43eff057fa55"),
                            UserId = new Guid("990c5df1-6f31-4975-9c0a-b65b07a8ecd9")
                        },
                        new
                        {
                            Id = new Guid("5d5f3846-d2af-4e28-a5ef-3bb50e7ee0d1"),
                            MotorcycleId = new Guid("45aa7ade-dd9f-47c4-a768-93af5d4ab2c3"),
                            ServiceId = new Guid("8ffd5261-a6e7-4c56-a21e-3346b42294b4")
                        },
                        new
                        {
                            Id = new Guid("38f4f8a7-e0d7-4e1a-a732-d8e88efea9a7"),
                            MotorcycleId = new Guid("e7408fde-2c8f-455e-8bdb-672ba5ee537c"),
                            ServiceId = new Guid("b958aadd-506a-4fb7-acac-ec88503ee279")
                        },
                        new
                        {
                            Id = new Guid("871fdb5e-1928-4919-b979-40bd810cd540"),
                            MotorcycleId = new Guid("fc876abb-9bef-4609-a6c7-5efa520d64a1"),
                            ServiceId = new Guid("8da8b3df-0123-446f-ad12-43eff057fa55")
                        });
                });

            modelBuilder.Entity("ServiMotoServer.Models.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ServiMotoServer.Models.TaskAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("AssignedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MotorcycleId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MotorcycleId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskAssignments");
                });

            modelBuilder.Entity("ServiMotoServer.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdministrator")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b1ffb1f6-71f8-42d2-8c54-31d3aff66863"),
                            Email = "admin1@admin.com",
                            IsAdministrator = true,
                            Password = "admin123",
                            Username = "admin1"
                        },
                        new
                        {
                            Id = new Guid("f2bd003f-d22d-4080-ab46-09db6b5e4ee7"),
                            Email = "admin2@admin.com",
                            IsAdministrator = true,
                            Password = "admin123",
                            Username = "admin2"
                        },
                        new
                        {
                            Id = new Guid("990c5df1-6f31-4975-9c0a-b65b07a8ecd9"),
                            Email = "admin3@admin.com",
                            IsAdministrator = true,
                            Password = "admin123",
                            Username = "admin3"
                        });
                });

            modelBuilder.Entity("ServiMotoServer.Models.ServiceAssignment", b =>
                {
                    b.HasOne("ServiMotoServer.Models.Motorcycle", "Motorcycle")
                        .WithMany("ServiceAssignments")
                        .HasForeignKey("MotorcycleId");

                    b.HasOne("ServiMotoServer.Models.Service", "Service")
                        .WithMany("ServiceAssignments")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiMotoServer.Models.User", "User")
                        .WithMany("ServiceAssignments")
                        .HasForeignKey("UserId");

                    b.Navigation("Motorcycle");

                    b.Navigation("Service");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServiMotoServer.Models.Task", b =>
                {
                    b.HasOne("ServiMotoServer.Models.Service", "Service")
                        .WithMany("Tasks")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("ServiMotoServer.Models.TaskAssignment", b =>
                {
                    b.HasOne("ServiMotoServer.Models.Motorcycle", "Motorcycle")
                        .WithMany("TaskAssignments")
                        .HasForeignKey("MotorcycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiMotoServer.Models.Task", "Task")
                        .WithMany("TaskAssignments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Motorcycle");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("ServiMotoServer.Models.Motorcycle", b =>
                {
                    b.Navigation("ServiceAssignments");

                    b.Navigation("TaskAssignments");
                });

            modelBuilder.Entity("ServiMotoServer.Models.Service", b =>
                {
                    b.Navigation("ServiceAssignments");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("ServiMotoServer.Models.Task", b =>
                {
                    b.Navigation("TaskAssignments");
                });

            modelBuilder.Entity("ServiMotoServer.Models.User", b =>
                {
                    b.Navigation("ServiceAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
