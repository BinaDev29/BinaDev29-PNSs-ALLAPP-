// File Path: Domain/Enums/NotificationStatus.cs
namespace Domain.Enums
{
    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed,
        Processing, // <-- ይህንን ጨምር
        PartiallySent // <-- ይህንን ጨምር
    }
}