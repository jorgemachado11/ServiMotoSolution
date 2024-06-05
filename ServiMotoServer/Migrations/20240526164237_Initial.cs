using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiMotoServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Motorcycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MotorcycleName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ServiceName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdministrator = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TaskName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MotorcycleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ServiceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAssignments_Motorcycles_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceAssignments_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TaskId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MotorcycleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Motorcycles_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAssignments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Motorcycles",
                columns: new[] { "Id", "Description", "MotorcycleName", "Password" },
                values: new object[,]
                {
                    { new Guid("45aa7ade-dd9f-47c4-a768-93af5d4ab2c3"), "Motorcycle 1", "motorcycle1", "moto123" },
                    { new Guid("e7408fde-2c8f-455e-8bdb-672ba5ee537c"), "Motorcycle 2", "motorcycle2", "moto123" },
                    { new Guid("fc876abb-9bef-4609-a6c7-5efa520d64a1"), "Motorcycle 3", "motorcycle3", "moto123" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "ServiceName" },
                values: new object[,]
                {
                    { new Guid("8da8b3df-0123-446f-ad12-43eff057fa55"), "Description for Service 3", "Service3" },
                    { new Guid("8ffd5261-a6e7-4c56-a21e-3346b42294b4"), "Description for Service 1", "Service1" },
                    { new Guid("b958aadd-506a-4fb7-acac-ec88503ee279"), "Description for Service 2", "Service2" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsAdministrator", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("990c5df1-6f31-4975-9c0a-b65b07a8ecd9"), "admin3@admin.com", true, "admin123", "admin3" },
                    { new Guid("b1ffb1f6-71f8-42d2-8c54-31d3aff66863"), "admin1@admin.com", true, "admin123", "admin1" },
                    { new Guid("f2bd003f-d22d-4080-ab46-09db6b5e4ee7"), "admin2@admin.com", true, "admin123", "admin2" }
                });

            migrationBuilder.InsertData(
                table: "ServiceAssignments",
                columns: new[] { "Id", "MotorcycleId", "ServiceId", "UserId" },
                values: new object[,]
                {
                    { new Guid("1f927b84-4ba6-43a5-b515-5d5b28cb7808"), null, new Guid("b958aadd-506a-4fb7-acac-ec88503ee279"), new Guid("f2bd003f-d22d-4080-ab46-09db6b5e4ee7") },
                    { new Guid("2aafec2b-9f37-410a-bfb4-185472997106"), null, new Guid("8da8b3df-0123-446f-ad12-43eff057fa55"), new Guid("990c5df1-6f31-4975-9c0a-b65b07a8ecd9") },
                    { new Guid("38f4f8a7-e0d7-4e1a-a732-d8e88efea9a7"), new Guid("e7408fde-2c8f-455e-8bdb-672ba5ee537c"), new Guid("b958aadd-506a-4fb7-acac-ec88503ee279"), null },
                    { new Guid("5d5f3846-d2af-4e28-a5ef-3bb50e7ee0d1"), new Guid("45aa7ade-dd9f-47c4-a768-93af5d4ab2c3"), new Guid("8ffd5261-a6e7-4c56-a21e-3346b42294b4"), null },
                    { new Guid("871fdb5e-1928-4919-b979-40bd810cd540"), new Guid("fc876abb-9bef-4609-a6c7-5efa520d64a1"), new Guid("8da8b3df-0123-446f-ad12-43eff057fa55"), null },
                    { new Guid("888f586d-5247-406d-9b9d-13e5cf088c7f"), null, new Guid("8ffd5261-a6e7-4c56-a21e-3346b42294b4"), new Guid("b1ffb1f6-71f8-42d2-8c54-31d3aff66863") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAssignments_MotorcycleId",
                table: "ServiceAssignments",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAssignments_ServiceId",
                table: "ServiceAssignments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAssignments_UserId",
                table: "ServiceAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceName",
                table: "Services",
                column: "ServiceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_MotorcycleId",
                table: "TaskAssignments",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignments_TaskId",
                table: "TaskAssignments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ServiceId",
                table: "Tasks",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceAssignments");

            migrationBuilder.DropTable(
                name: "TaskAssignments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Motorcycles");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
