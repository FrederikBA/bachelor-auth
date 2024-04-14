using Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data;

public class AuthContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Primary keys
        modelBuilder.Entity<User>().HasKey(order => order.Id);
        
        //Add Value Objects
        modelBuilder.Entity<Role>(ConfigureAddress);
        
        //Configure properties
        //Unique Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        
        //Configure relationships
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
    }
    
    //Role value object
    void ConfigureAddress<T>(EntityTypeBuilder<T> entity) where T : Role
    {
        entity.ToTable("Role", "dbo");

        entity.Property<int>("Id")
            .IsRequired();
        entity.HasKey("Id");
    }
}