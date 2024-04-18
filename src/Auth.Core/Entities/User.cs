using Shared.Integration.Interfaces;
using Shared.Integration.Models;

namespace Auth.Core.Entities;

public class User : BaseEntity, IAggregateRoot
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}