using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UntoldApp.Models
{
    public class ShowContext : DbContext
    {
        public ShowContext(DbContextOptions<ShowContext> options) : base(options)
        {

        }

        public DbSet<ShowModel> Show { get; set; }
    }
}
