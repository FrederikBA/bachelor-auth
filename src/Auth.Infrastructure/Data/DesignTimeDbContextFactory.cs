using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Shared.Integration.Configuration;

namespace Auth.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        const string connectionString = Constants.ConnectionStrings.ShwUsers; 
        var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AuthContext(optionsBuilder.Options);
    }
}