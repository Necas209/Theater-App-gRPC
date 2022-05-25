using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class User
{
    public User()
    {
        Logs = new HashSet<Log>();
    }
    
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [StringLength(256)]
    public string UserName { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [InverseProperty(nameof(GrpcLibrary.Models.Admin.User))]
    public Admin? Admin { get; set; }

    [InverseProperty(nameof(GrpcLibrary.Models.Client.User))]
    public Client? Client { get; set; }

    [InverseProperty(nameof(GrpcLibrary.Models.Manager.User))]
    public Manager? Manager { get; set; }
    
    [InverseProperty(nameof(Log.User))]
    public ICollection<Log> Logs { get; set; }
}