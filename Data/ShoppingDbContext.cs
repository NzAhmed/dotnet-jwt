using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using webapi2.Models;

namespace webapi2.Data
{
    public class ShoppingDbContext : DbContext
    {
        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products {get; set;}
    }
}
