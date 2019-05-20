using Alinta.Customers.API.Controllers;
using Alinta.Customers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Alinta.Customers.API.Tests
{
    [TestCaseOrderer("Alinta.Customers.API.Tests.PriorityOrderer", "Alinta.Customers.API.Tests")]
    public class CustomersControllerTests
    {
        CustomersController _controller;
        IDataRepository _repository;
        AlintaDbContext _context;

        public CustomersControllerTests()
        {
            _context = new AlintaDbContext(new DbContextOptionsBuilder<AlintaDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options);
            _repository = new DataRepository(_context);
            _controller = new CustomersController(_repository);
        }

        [Fact, TestPriority(1)]
        public async Task VerifyAddingCustomer()
        {
            var customerToAdd1 = new Customer()
            {
                FirstName = "Therese",
                LastName = "Missiha",
                BirthDate = new System.DateTime(1982, 10, 1)
            };

            var res = await _controller.PostCustomer(customerToAdd1);
            Assert.IsType<CreatedAtActionResult>(res.Result);
            Assert.Equal(1, customerToAdd1.CustomerId);

            var customerToAdd2 = new Customer()
            {
                FirstName = "Tamer",
                LastName = "Missiha",
                BirthDate = new System.DateTime(1980, 10, 17)
            };

            var res2 = await _controller.PostCustomer(customerToAdd2);
            Assert.IsType<CreatedAtActionResult>(res.Result);
            Assert.Equal(2, customerToAdd2.CustomerId);
        }

        [Fact, TestPriority(2)]
        public async Task VerifyGetAllCustomers()
        {
            var res = await _controller.GetCustomers();
            Assert.IsType<OkObjectResult>(res.Result);
            Assert.IsType<List<Customer>>(((OkObjectResult)res.Result).Value);
            Assert.Equal(2, ((List<Customer>)((OkObjectResult)res.Result).Value).Count);
        }

        [Fact, TestPriority(3)]
        public async Task VerifyUpdateCustomerSuccess()
        {
            var customerToEdit = new Customer()
            {
                CustomerId = 1,
                FirstName = "Therese",
                LastName = "William",
                BirthDate = new System.DateTime(1982, 10, 1)
            };
            var editRes = await _controller.PutCustomer(1, customerToEdit);
            Assert.IsType<OkObjectResult>(editRes);

            var res = await _controller.GetCustomers();
            Assert.Equal("William", ((List<Customer>)((OkObjectResult)res.Result).Value)[0].LastName);
        }


        [Fact, TestPriority(4)]
        public async Task VerifyUpdateCustomerNotFound()
        {
            var customerToEdit = new Customer()
            {
                CustomerId = 100,
                FirstName = "Therese",
                LastName = "William",
                BirthDate = new System.DateTime(1982, 10, 1)
            };
            var editRes = await _controller.PutCustomer(100, customerToEdit);
            Assert.IsType<NotFoundResult>(editRes);
        }

        [Fact, TestPriority(5)]
        public async Task VerifyDeleteCustomerSuccess()
        {
            var delRes = await _controller.DeleteCustomer(1);
            Assert.IsType<OkObjectResult>(delRes.Result);

            var res = await _controller.GetCustomers();
            Assert.All((List<Customer>)((OkObjectResult)res.Result).Value,customer => Assert.NotEqual(1,customer.CustomerId));
        }
        [Fact, TestPriority(5)]
        public async Task VerifyDeleteCustomerNotFound()
        {
            var delRes = await _controller.DeleteCustomer(100);
            Assert.IsType<NotFoundResult>(delRes.Result);
        }
    }
}
