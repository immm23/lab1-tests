using LabOOP2.DAL;
using LabOOP2.Domain;
using LabOOP2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly Context _context;

        public BankAccountsController(Context context)
        {
            _context = context;
        }

        [HttpGet("bankAccounts")]
        public BankAccount[] GetBankAccounts()
        {
            return _context.BankAccounts.ToArray();
        }

        [HttpGet("customers/{id}/bankAccount")]
        public IActionResult GetBankAccount(Guid id)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }

            return Ok(dbCustomer.BankAccount);
        }

        [HttpPost("customers/{id}/bankAccount")]
        public IActionResult Create([FromRoute]Guid id, BankAccountInputModel bankAccountInputModel)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            var bankAccount = new BankAccount(
                Guid.NewGuid(),
                bankAccountInputModel.CardNumber,
                bankAccountInputModel.ExpiryDate,
                bankAccountInputModel.CVV);

            customer.SetBankAccount(bankAccount);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("customers/{id}/bankAccount")]
        public IActionResult Update(Guid id, BankAccountInputModel bankAccountInputModel)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }
            else if (customer.BankAccount is null)
            {
                return BadRequest("Can not update item that is not existing");
            }

            var bankAccount = new BankAccount(
                Guid.NewGuid(),
                bankAccountInputModel.CardNumber,
                bankAccountInputModel.ExpiryDate,
                bankAccountInputModel.CVV);

            customer.SetBankAccount(bankAccount);
            _context.SaveChanges();
            return Ok();
        }
    }
}
