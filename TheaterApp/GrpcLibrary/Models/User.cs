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

    public User()
    {
        Name = "";
        UserName = "";
        Email = "";
        PasswordHash = "";
        Logs = new HashSet<Log>();
    }

    [Key] public int Id { get; set; }

    public string Name { get; set; }

    [StringLength(256)] public string UserName { get; set; }

    [DataType(DataType.EmailAddress)]
    [StringLength(256)]
    public string Email { get; set; }

    [StringLength(64)] public string PasswordHash { get; set; }

    public Admin? Admin { get; set; }

    public Client? Client { get; set; }

    public Manager? Manager { get; set; }

    public ICollection<Log> Logs { get; set; }
}