using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class User
{
    public enum UserType
    {
        Client,
        Admin,
        Manager
    }

    public User()
    {
        Logs = new HashSet<Log>();
    }

    [Key] public int Id { get; set; }

    public string Name { get; set; } = null!;

    [StringLength(256)] public string UserName { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [StringLength(64)] public string PasswordHash { get; set; } = null!;

    [InverseProperty(nameof(Models.Admin.User))]
    public Admin? Admin { get; set; }

    [InverseProperty(nameof(Models.Client.User))]
    public Client? Client { get; set; }

    [InverseProperty(nameof(Models.Manager.User))]
    public Manager? Manager { get; set; }

    [InverseProperty(nameof(Log.User))] public ICollection<Log> Logs { get; set; }
}