using Microsoft.EntityFrameworkCore;
using Zadanie5_test.Models;

namespace Zadanie5_test.Data
{
    public class Zadanie5_testContext : DbContext
    {
        public Zadanie5_testContext(DbContextOptions<Zadanie5_testContext> options) : base(options) { }
        public DbSet<Klient> klienci { get; set; }
    }
}
