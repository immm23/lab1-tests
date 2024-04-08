using LabOOP2.DAL;
using LabOOP2.Domain;
using LabOOP2.Domain.Services;
using LabOOP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly Context _context;
        private readonly IPaymentService _paymentService;

        public CustomersController(Context context,
            IPaymentService service)
        {
            _context = context;
            _paymentService = service;
        }

        [HttpGet]
        public Customer[] GetCustomers()
        {
            return _context.Customers
                .Include(p => p.Transactions)
                .Include(p => p.Loans)
                .Include(p => p.Passport)
                .Include(p => p.BankAccount)
                .ToArray();
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer(Guid id)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.Transactions)
                .Include(p => p.Loans)
                .Include(p => p.Passport)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }

            return Ok(dbCustomer);
        }

        [HttpPost]
        public IActionResult Create(CustomerInputModel customerInput)
        {
            var customer = new Customer(
                Guid.NewGuid(),
                customerInput.Name,
                customerInput.Surname,
                customerInput.Address);

            _context.Add(customer);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(CustomerInputModel customerInput)
        {
            var customer = new Customer(
                customerInput.Id,
                customerInput.Name,
                customerInput.Surname,
                customerInput.Address);

            var dbCustomer = _context.Customers.Where(p => p.Id == customer.Id)
                .FirstOrDefault();
            if(dbCustomer is null)
            {
                return NotFound();
            }
            _context.Entry(dbCustomer).State = EntityState.Detached;

            _context.Update(customer);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
                .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            _context.Remove(customer);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("{id}/balance")]
        public IActionResult MoveBalance(Guid id, decimal amount)
        {
            var customer = _context.Customers.Where(p => p.Id == id)
                .FirstOrDefault();

            if (customer is null)
            {
                return NotFound();
            }

            try
            {
                customer.MoveBalance(amount, _paymentService);
                _context.SaveChanges();

                return Ok();
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
