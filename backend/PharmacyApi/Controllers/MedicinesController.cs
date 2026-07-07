using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyApi.Data;
using PharmacyApi.DTOs;
using PharmacyApi.Models;

namespace PharmacyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicinesController : ControllerBase
    {
        private readonly PharmacyContext _context;
        private const int NearExpiryThresholdDays = 30;
        private const int LowStockThreshold = 10;

        public MedicinesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/medicines?search=para
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineListDto>>> GetMedicines([FromQuery] string? search)
        {
            var query = _context.Medicines.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m => EF.Functions.Like(m.FullName, $"%{search}%"));
            }

            var medicines = await query
                .OrderBy(m => m.FullName)
                .ToListAsync();

            var result = medicines.Select(m => ToListDto(m)).ToList();

            return Ok(result);
        }

        // GET: api/medicines/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicineDetailDto>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound();

            return Ok(new MedicineDetailDto
            {
                Id = medicine.Id,
                FullName = medicine.FullName,
                Notes = medicine.Notes,
                ExpiryDate = medicine.ExpiryDate,
                Quantity = medicine.Quantity,
                Price = medicine.Price,
                Brand = medicine.Brand
            });
        }

        // POST: api/medicines
        [HttpPost]
        public async Task<ActionResult<MedicineDetailDto>> CreateMedicine(CreateMedicineDto dto)
        {
            var medicine = new Medicine
            {
                FullName = dto.FullName,
                Notes = dto.Notes,
                ExpiryDate = dto.ExpiryDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Brand = dto.Brand
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            var result = new MedicineDetailDto
            {
                Id = medicine.Id,
                FullName = medicine.FullName,
                Notes = medicine.Notes,
                ExpiryDate = medicine.ExpiryDate,
                Quantity = medicine.Quantity,
                Price = medicine.Price,
                Brand = medicine.Brand
            };

            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, result);
        }

        // PUT: api/medicines/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMedicine(int id, UpdateMedicineDto dto)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound();

            medicine.FullName = dto.FullName;
            medicine.Notes = dto.Notes;
            medicine.ExpiryDate = dto.ExpiryDate;
            medicine.Quantity = dto.Quantity;
            medicine.Price = dto.Price;
            medicine.Brand = dto.Brand;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/medicines/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return NotFound();

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static MedicineListDto ToListDto(Medicine m) => new()
        {
            Id = m.Id,
            FullName = m.FullName,
            ExpiryDate = m.ExpiryDate,
            Quantity = m.Quantity,
            Price = m.Price,
            Brand = m.Brand,
            IsNearExpiry = (m.ExpiryDate.Date - DateTime.UtcNow.Date).TotalDays < NearExpiryThresholdDays,
            IsLowStock = m.Quantity < LowStockThreshold
        };
    }
}
