using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyApi.Models
{
    public class SaleRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [ForeignKey(nameof(MedicineId))]
        public Medicine? Medicine { get; set; }

        [Required]
        public int QuantitySold { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPriceAtSale { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public string? Buyer { get; set; }
    }
}
