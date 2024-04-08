using LabOOP2.DAL;
using LabOOP2.Domain;
using LabOOP2.Domain.Services;
using LabOOP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILoanRandomizer _loanRandomizer;

        public LoansController(Context context,
            ILoanRandomizer loanRandomizer)
        {
            _context = context;
            _loanRandomizer = loanRandomizer;
        }

        [HttpGet("customer/{id}/loans")]
        public IActionResult GetLoans(Guid id)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
               .Include(p => p.Loans)
               .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer.Loans.ToArray());
        }

        [HttpPost("customer/{id}/loans")]
        public IActionResult Create(Guid id, decimal amount)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
               .Include(p => p.Passport)
               .Include(p => p.BankAccount)
               .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            try
            {
                customer.TakeLoan(amount, _loanRandomizer);

                _context.SaveChanges();
                return Created(nameof(Create), null);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("customers/{customerId}/loans/{loanId}")]
        public IActionResult PayOff(Guid customerId, Guid loanId, decimal amount)
        {
            var customer = _context.Customers.Where(p => p.Id == customerId)
                .Include(p => p.Loans)
               .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            try
            {
                customer.PayOffLoan(customerId, amount);
                _context.SaveChanges();
                return Ok();
            }
            catch(ArgumentNullException)
            {
                return NotFound();
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
