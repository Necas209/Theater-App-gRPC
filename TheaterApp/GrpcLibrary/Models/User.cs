using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class User
{
    public enum UserType
    {
        Client,
        Admin,
        Manager
    }

    [Key] public int Id { get; set; }

    public required string Name { get; set; }

    [StringLength(256)] public required string UserName { get; set; }

    [DataType(DataType.EmailAddress)]
    [StringLength(256)]
    public required string Email { get; set; }

    [StringLength(64)] public required string PasswordHash { get; set; }

    public Admin? Admin { get; set; }

    public Client? Client { get; init; }

    public Manager? Manager { get; set; }

    // ReSharper disable once CollectionNeverQueried.Global
    public ICollection<Log> Logs { get; init; } = new HashSet<Log>();
}