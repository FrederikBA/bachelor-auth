namespace Auth.Core.Entities;

public class Role //Value Object
{
    public RoleTypes RoleType { get; set; }    
    public List<User> Users { get; set; } = null!;
}