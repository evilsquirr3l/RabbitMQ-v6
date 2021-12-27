using Microsoft.EntityFrameworkCore;
using Subscriber.Models;

namespace Subscriber;

public class GunsDbContext : DbContext
{
    public GunsDbContext(DbContextOptions<GunsDbContext> options) : base(options)
    {
        
    }

    public DbSet<Gun> Guns { get; set; }
}