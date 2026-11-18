namespace UserService.Domain.Enums;

public enum UserAccountStatus
{
    PendingActivation = 0,
    Active = 1,
    Suspended = 2,
    PendingDeletion = 3,
    Deleted = 4,
    Blocked = 5
}