using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Alinta.Customers.Data;

namespace Alinta.Customers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IDataRepository _repository;

        public CustomersController(IDataRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return Ok(await _repository.GetCustomers());
        }

        // GET: api/Customers
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _repository.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET: api/SearchCustomers/searchtext
        [HttpGet("{searchText}")]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers(string searchText)
        {
            var customers = await _repository.SearchCustomersByName(searchText);

            if (customers == null || customers.Count == 0)
            {
                return NotFound();
            }

            return Ok(customers);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _repository.UpdateEntity(customer);

            try
            {
                await _repository.SaveAllAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _repository.AddEntity(customer);
            await _repository.SaveAllAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _repository.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            _repository.RemoveEntity(customer);
            await _repository.SaveAllAsync();

            return Ok("Customer deleted.");
        }
    }
}
