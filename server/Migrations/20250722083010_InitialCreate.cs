using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Yes.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Courtesy_title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Full_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Phone_number = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Contact_info = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Role_id = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_id",
                        column: x => x.Role_id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    RoomType_id = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomTypes_RoomType_id",
                        column: x => x.RoomType_id,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomTickets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Customer_id = table.Column<string>(type: "TEXT", nullable: false),
                    Room_id = table.Column<string>(type: "TEXT", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Number_of_occupants = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomTickets_Customers_Customer_id",
                        column: x => x.Customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomTickets_Rooms_Room_id",
                        column: x => x.Room_id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTickets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Customer_id = table.Column<string>(type: "TEXT", nullable: false),
                    Room_id = table.Column<string>(type: "TEXT", nullable: false),
                    Service_id = table.Column<string>(type: "TEXT", nullable: false),
                    Details = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTickets_Customers_Customer_id",
                        column: x => x.Customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTickets_Rooms_Room_id",
                        column: x => x.Room_id,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTickets_Services_Service_id",
                        column: x => x.Service_id,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Age", "Contact_info", "Courtesy_title", "Full_name", "Phone_number" },
                values: new object[,]
                {
                    { "C_0001", 30, null, "Mr.", "John Doe", "1234567890" },
                    { "C_0002", 28, null, "Ms.", "Jane Smith", "0987654321" },
                    { "C_0003", 35, null, "Mrs.", "Emily Johnson", "1112223333" },
                    { "C_0004", 40, null, "Mr.", "Michael Brown", "2223334444" },
                    { "C_0005", 27, null, "Ms.", "Linda Davis", "3334445555" },
                    { "C_0006", 50, null, "Dr.", "Robert Wilson", "4445556666" },
                    { "C_0007", 22, null, "Miss", "Sophia Martinez", "5556667777" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { "R_001", "Manager" },
                    { "R_002", "Receptionist" },
                    { "R_003", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { "RT_001", "A room for one person, equipped with a single bed.", "Single", 500000 },
                    { "RT_002", "A room for two people, equipped with a double bed.", "Double", 750000 },
                    { "RT_003", "A more spacious room with additional amenities, suitable for couples or small families.", "Deluxe", 900000 },
                    { "RT_004", "A luxurious room with separate living and sleeping areas, ideal for longer stays or special occasions.", "Suite", 1000000 }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { "SVC_001", "Cleaning", 0 },
                    { "SVC_002", "Laundry", 0 },
                    { "SVC_003", "Spa", 75000 },
                    { "SVC_004", "Restaurant", 0 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Label", "RoomType_id", "Status" },
                values: new object[,]
                {
                    { "RM_001", "A1", "RT_001", 0 },
                    { "RM_002", "A2", "RT_001", 0 },
                    { "RM_003", "A3", "RT_001", 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Description", "Password", "Role_id", "Username" },
                values: new object[,]
                {
                    { "U_001", "Hotel Manager", "$2a$11$.2V271Z9m2yiNjX1K/.jhefV3.fthIA1c4bZFZIxW6sVXFQACxhEu", "R_001", "Manager" },
                    { "U_002", "Front Desk Receptionist", "$2a$11$TNYb8jcMW1w2aBefYxE8Q.3vD0HB/JXJkLtQXftzxxBadYtKfXwHe", "R_002", "Receptionist" },
                    { "U_003", "Hotel Staff", "$2a$11$aIRdRpgVNW98VT33HB.Ohuc7NuXIUOm8ROr5PSg9hSsdwmPG4gu8y", "R_003", "Staff" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomType_id",
                table: "Rooms",
                column: "RoomType_id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTickets_Customer_id",
                table: "RoomTickets",
                column: "Customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTickets_Room_id",
                table: "RoomTickets",
                column: "Room_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTickets_Customer_id",
                table: "ServiceTickets",
                column: "Customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTickets_Room_id",
                table: "ServiceTickets",
                column: "Room_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTickets_Service_id",
                table: "ServiceTickets",
                column: "Service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_id",
                table: "Users",
                column: "Role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomTickets");

            migrationBuilder.DropTable(
                name: "ServiceTickets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "RoomTypes");
        }
    }
}
