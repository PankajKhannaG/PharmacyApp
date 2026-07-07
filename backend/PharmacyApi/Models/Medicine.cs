using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyApi.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Notes { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(150)]
        public string Brand { get; set; } = string.Empty;

        // Navigation
        public ICollection<SaleRecord> SaleRecords { get; set; } = new List<SaleRecord>();
    }
}
