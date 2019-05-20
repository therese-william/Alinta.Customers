using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alinta.Customers.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly AlintaDbContext _context;

        public DataRepository(AlintaDbContext context)
        {
            _context = context;
        }
        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        public Task<Customer> GetCustomer(int customerId)
        {
            return _context.Customers.FindAsync(customerId);
        }

        public Task<List<Customer>> GetCustomers()
        {
            return _context.Customers.ToListAsync();
        }

        public void RemoveEntity(object model)
        {
            _context.Remove(model);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public Task<int> SaveAllAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<List<Customer>> SearchCustomersByName(string searchText)
        {
            return _context.Customers.Where(c => c.FirstName.ToLower().Contains(searchText.ToLower()) || c.LastName.ToLower().Contains(searchText.ToLower())).ToListAsync();
        }

        public void UpdateEntity(object model)
        {
            _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        }

    }
}
