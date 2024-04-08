using LabOOP2.DAL;
using LabOOP2.Domain;
using LabOOP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PassportsController : ControllerBase
    {
        private readonly Context _context;

        public PassportsController(Context context)
        {
            _context = context;
        }

        [HttpGet("passports")]
        public Passport[] GetPassports()
        {
            return _context.Passports.ToArray();
        }

        [HttpGet("customers/{id}/passport")]
        public IActionResult GetPassport(Guid id)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.Passport)
                .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer.Passport);
        }

        [HttpPost("customers/{id}/passport")]
        public IActionResult Create(Guid id, PassportInputModel passportInputModel)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
               .Include(p => p.Passport)
               .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            var passport = new Passport(
                Guid.NewGuid(),
                passportInputModel.SerialNumber,
                passportInputModel.IssuedByAuthority,
                passportInputModel.IssuedDate,
                passportInputModel.ExpiryDate);

            customer.SetPassport(passport);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("customers/{id}/passport")]
        public IActionResult Update(Guid id, PassportInputModel passportInputModel)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
               .Include(p => p.Passport)
               .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }
            else if (customer.Passport is null){
                return BadRequest("Can not update passport which is not created");
            }

            var passport = new Passport(
                passportInputModel.Id,
                passportInputModel.SerialNumber,
                passportInputModel.IssuedByAuthority,
                passportInputModel.IssuedDate,
                passportInputModel.ExpiryDate);

            customer.SetPassport(passport);
            _context.SaveChanges();
            return Ok();
        }
    }
}
