using Microsoft.EntityFrameworkCore;
using sebtaskAPI.Models;

namespace sebtaskAPI.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<ExchangeRate> ExchangeRates { get; set; }

    
  }
  
}
