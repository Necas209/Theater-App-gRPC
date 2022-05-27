using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace GrpcLibrary.Models;

public sealed class User
{
    public User()
    {
        Logs = new HashSet<Log>();
    }
    
    public static string HashPassword(string password)
    {
        using var mySha256 = SHA256.Create();
        var pwBytes = Encoding.UTF8.GetBytes(password);
        var pwHash = mySha256.ComputeHash(pwBytes);
        return Convert.ToHexString(pwHash).ToLower();
    }

    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [StringLength(256)]
    public string UserName { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [StringLength(256)]
    public string Email { get; set; } = null!;
    
    [StringLength(64)]
    public string PasswordHash { get; set; } = null!;

    [InverseProperty(nameof(GrpcLibrary.Models.Admin.User))]
    public Admin? Admin { get; set; }

    [InverseProperty(nameof(GrpcLibrary.Models.Client.User))]
    public Client? Client { get; set; }

    [InverseProperty(nameof(GrpcLibrary.Models.Manager.User))]
    public Manager? Manager { get; set; }
    
    [InverseProperty(nameof(Log.User))]
    public ICollection<Log> Logs { get; set; }
}