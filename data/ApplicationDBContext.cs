using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.data
{
    public class ApplicationDBContext : DbContext
    {
      public ApplicationDBContext(DbContextOptions dbContextOptions): base(dbContextOptions)
      {

      }

      public DbSet<Stocks> Stocks { get; set; }
      public DbSet<Comment> Comments{ get; set; }
    }
}
