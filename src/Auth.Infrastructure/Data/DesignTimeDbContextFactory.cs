using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Auth.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        //TODO: Move connection string to appsettings.json / configuration file
        const string connectionString = "";
        
        var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AuthContext(optionsBuilder.Options);
    }
}