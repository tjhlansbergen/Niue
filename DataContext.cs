using Microsoft.EntityFrameworkCore;

namespace Niue;

public class DataContext : DbContext
{
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Log> Logs => Set<Log>();

    private readonly IConfiguration _configuration;

    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_configuration.GetConnectionString("Niue"));
    }
}