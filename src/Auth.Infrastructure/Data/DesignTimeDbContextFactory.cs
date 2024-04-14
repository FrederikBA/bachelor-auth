using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Auth.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        //TODO: Move connection string to appsettings.json / configuration file
        const string connectionString = "Server=mssql;Database=ShwUsers;User Id=sa;Password=thisIsSuperStrong1234;TrustServerCertificate=True";
        
        var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AuthContext(optionsBuilder.Options);
    }
}