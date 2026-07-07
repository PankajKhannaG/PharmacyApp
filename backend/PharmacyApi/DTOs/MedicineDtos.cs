using System.ComponentModel.DataAnnotations;

namespace PharmacyApi.DTOs
{
    // Used for the grid listing - Notes intentionally excluded per requirement
    public class MedicineListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; } = string.Empty;

        // Computed flags so the frontend doesn't need to re-derive date math
        public bool IsNearExpiry { get; set; }
        public bool IsLowStock { get; set; }
    }

    // Full detail (includes Notes) - used for single-record view/edit
    public class MedicineDetailDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; } = string.Empty;
    }

    public class CreateMedicineDto
    {
        [Required, MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Notes { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Required, MaxLength(150)]
        public string Brand { get; set; } = string.Empty;
    }

    public class UpdateMedicineDto : CreateMedicineDto
    {
    }

    public class CreateSaleDto
    {
        [Required]
        public int MedicineId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int QuantitySold { get; set; }

        public string? Buyer { get; set; }
    }

    public class SaleRecordDto
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal UnitPriceAtSale { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public string? Buyer { get; set; }
    }
}
