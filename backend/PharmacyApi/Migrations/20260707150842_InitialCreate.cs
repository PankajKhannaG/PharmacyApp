using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmacyApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false),
                    UnitPriceAtSale = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Buyer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleRecords_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "Id", "Brand", "ExpiryDate", "FullName", "Notes", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "Cipla", new DateTime(2026, 11, 4, 15, 8, 39, 376, DateTimeKind.Utc).AddTicks(9755), "Paracetamol 500mg", "For fever and mild pain relief", 25.50m, 50 },
                    { 2, "Sun Pharma", new DateTime(2026, 7, 22, 15, 8, 39, 376, DateTimeKind.Utc).AddTicks(9781), "Amoxicillin 250mg", "Antibiotic - complete full course", 120.00m, 8 },
                    { 3, "Dr. Reddy's", new DateTime(2027, 1, 23, 15, 8, 39, 376, DateTimeKind.Utc).AddTicks(9787), "Cetirizine 10mg", "Antihistamine for allergies", 15.75m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_FullName",
                table: "Medicines",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_SaleRecords_MedicineId",
                table: "SaleRecords",
                column: "MedicineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleRecords");

            migrationBuilder.DropTable(
                name: "Medicines");
        }
    }
}
