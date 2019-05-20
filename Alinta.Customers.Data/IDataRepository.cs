using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alinta.Customers.Data
{
    public interface IDataRepository
    {
        Task<List<Customer>> GetCustomers();
        Task<List<Customer>> SearchCustomersByName(string searchText);
        Task<Customer> GetCustomer(int customerId);
        void RemoveEntity(object model);
        void AddEntity(object model);
        void UpdateEntity(object model);
        bool SaveAll();
        Task<int> SaveAllAsync();
        bool CustomerExists(int id);
    }
}
