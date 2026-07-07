using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyApi.Data;
using PharmacyApi.DTOs;
using PharmacyApi.Models;

namespace PharmacyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public SalesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleRecordDto>>> GetSales()
        {
            var sales = await _context.SaleRecords
                .Include(s => s.Medicine)
                .OrderByDescending(s => s.SaleDate)
                .Select(s => new SaleRecordDto
                {
                    Id = s.Id,
                    MedicineId = s.MedicineId,
                    MedicineName = s.Medicine != null ? s.Medicine.FullName : string.Empty,
                    QuantitySold = s.QuantitySold,
                    UnitPriceAtSale = s.UnitPriceAtSale,
                    TotalAmount = s.TotalAmount,
                    SaleDate = s.SaleDate,
                    Buyer = s.Buyer
                })
                .ToListAsync();

            return Ok(sales);
        }

        // POST: api/sales
        // Records a sale and decrements the medicine's stock quantity.
        [HttpPost]
        public async Task<ActionResult<SaleRecordDto>> RecordSale(CreateSaleDto dto)
        {
            var medicine = await _context.Medicines.FindAsync(dto.MedicineId);
            if (medicine == null)
            {
                return NotFound($"Medicine with id {dto.MedicineId} not found.");
            }

            if (medicine.Quantity < dto.QuantitySold)
            {
                return BadRequest($"Insufficient stock. Available quantity: {medicine.Quantity}.");
            }

            var sale = new SaleRecord
            {
                MedicineId = medicine.Id,
                QuantitySold = dto.QuantitySold,
                UnitPriceAtSale = medicine.Price,
                TotalAmount = medicine.Price * dto.QuantitySold,
                SaleDate = DateTime.UtcNow,
                Buyer = dto.Buyer
            };

            medicine.Quantity -= dto.QuantitySold;

            _context.SaleRecords.Add(sale);
            await _context.SaveChangesAsync();

            var result = new SaleRecordDto
            {
                Id = sale.Id,
                MedicineId = sale.MedicineId,
                MedicineName = medicine.FullName,
                QuantitySold = sale.QuantitySold,
                UnitPriceAtSale = sale.UnitPriceAtSale,
                TotalAmount = sale.TotalAmount,
                SaleDate = sale.SaleDate,
                Buyer = sale.Buyer
            };

            return CreatedAtAction(nameof(GetSales), result);
        }
    }
}
