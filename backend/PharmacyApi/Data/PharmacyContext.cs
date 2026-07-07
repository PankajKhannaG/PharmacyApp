using Microsoft.EntityFrameworkCore;
using PharmacyApi.Models;

namespace PharmacyApi.Data
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options) { }

        public DbSet<Medicine> Medicines => Set<Medicine>();
        public DbSet<SaleRecord> SaleRecords => Set<SaleRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicine>().HasIndex(m => m.FullName);

            modelBuilder.Entity<SaleRecord>()
                .HasOne(s => s.Medicine)
                .WithMany(m => m.SaleRecords)
                .HasForeignKey(s => s.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed a few sample medicines so the grid isn't empty on first run
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine
                {
                    Id = 1,
                    FullName = "Paracetamol 500mg",
                    Notes = "For fever and mild pain relief",
                    ExpiryDate = DateTime.UtcNow.AddDays(120),
                    Quantity = 50,
                    Price = 25.50m,
                    Brand = "Cipla"
                },
                new Medicine
                {
                    Id = 2,
                    FullName = "Amoxicillin 250mg",
                    Notes = "Antibiotic - complete full course",
                    ExpiryDate = DateTime.UtcNow.AddDays(15),
                    Quantity = 8,
                    Price = 120.00m,
                    Brand = "Sun Pharma"
                },
                new Medicine
                {
                    Id = 3,
                    FullName = "Cetirizine 10mg",
                    Notes = "Antihistamine for allergies",
                    ExpiryDate = DateTime.UtcNow.AddDays(200),
                    Quantity = 3,
                    Price = 15.75m,
                    Brand = "Dr. Reddy's"
                }
            );
        }
    }
}
