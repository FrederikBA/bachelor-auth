using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}