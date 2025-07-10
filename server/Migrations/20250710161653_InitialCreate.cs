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
                table: "Users",
                columns: new[] { "Id", "Description", "Password", "Role_id", "Username" },
                values: new object[,]
                {
                    { "U_001", "Hotel Manager", "$2a$11$dHB6CXy/IQe09Bncrvyrp.CAaBcmpw4u0N.0lNl7qspLjwNa7CV6a", "R_001", "Manager" },
                    { "U_002", "Front Desk Receptionist", "$2a$11$PSG4fRx8q6RBM5elp3tYO.2ZriMV6CgEPmgvMcCK1.UUHhmtdx95e", "R_002", "Receptionist" },
                    { "U_003", "Hotel Staff", "$2a$11$LROwwHR8HZYHGps41oPzMe7Mtn0hpTtHJlKCRHYwsNhgkffyVDaeW", "R_003", "Staff" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_id",
                table: "Users",
                column: "Role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
