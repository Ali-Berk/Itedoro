using Itedoro.Domain.Constants;
using Itedoro.Domain.Entities.Roles;
using Itedoro.Domain.Exceptions;

namespace Itedoro.Domain.Entities.Users;

public class User
{
    public Guid Id { get; init; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string? Name { get; private set; }
    public string? Surname { get; private set; }

    public DateTime CreatedAt { get; private init; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public Guid RoleId { get; private set; }
    public virtual Role Role { get; private set; } = null!;

    public User(
        string username,
        string email,
        string passwordHash,
        Guid id = default,
        Guid roleId = default,
        DateTime createdAt = default,
        DateTime? updatedAt = null,
        string? name = null,
        string? surname = null,
        bool isDeleted = false)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Username = DomainException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        Email = ValidateEmail(email);
        PasswordHash = DomainException.ThrowIfNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        RoleId = roleId == Guid.Empty ? RoleIds.User : roleId;
        Name = NormalizeOptional(name);
        Surname = NormalizeOptional(surname);
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
        UpdatedAt = updatedAt;
        IsDeleted = isDeleted;
    }
    public void UpdateProfile(string? name, string? surname)
    {
        EnsureActive();
        Name = string.IsNullOrWhiteSpace(name) ? Name : NormalizeOptional(name);
        Surname = string.IsNullOrWhiteSpace(surname) ? Surname : NormalizeOptional(surname);
        Touch();
    }
    public void UpdatePasswordHash(string newPasswordHash)
    {
        EnsureActive();
        PasswordHash = DomainException.ThrowIfNullOrWhiteSpace(newPasswordHash, nameof(newPasswordHash));
        Touch();
    }

    public void AssignRole(Guid roleId)
    {
        EnsureActive();
        RoleId = DomainException.ThrowIfEmpty(roleId, nameof(roleId));
        Touch();
    }

    public void SoftDelete()
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;
        Touch();
    }

    public void Restore()
    {
        if (!IsDeleted)
        {
            return;
        }

        IsDeleted = false;
        Touch();
    }

    private void EnsureActive()
    {
        DomainException.ThrowIf(IsDeleted, "Deleted users cannot be modified.");
    }

    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string ValidateEmail(string email)
    {
        var normalized = DomainException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        DomainException.ThrowIf(!normalized.Contains('@', StringComparison.Ordinal), "email must be a valid email address.");
        return normalized;
    }
}
