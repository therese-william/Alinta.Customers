using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alinta.Customers.Data
{
    public class AlintaDbContext : DbContext
    {
        public AlintaDbContext(DbContextOptions<AlintaDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }


    }
}
