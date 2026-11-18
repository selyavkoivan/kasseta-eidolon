using Microsoft.AspNetCore.Identity;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public UserAccountStatus Status { get; set; } = UserAccountStatus.Active;

    // Навигационное свойство на бизнес-профиль (один-к-одному)
    public UserProfile Profile { get; set; } = null!;
}
