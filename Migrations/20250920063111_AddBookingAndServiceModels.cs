using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingAndServiceModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentTime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BarberName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpecialRequests = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
