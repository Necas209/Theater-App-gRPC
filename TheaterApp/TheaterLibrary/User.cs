using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public sealed class User
{
    public User()
    {
        Logs = new HashSet<Log>();
    }
    
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    [StringLength(256)]
    public string UserName { get; set; }
    
    [DataType(DataType.EmailAddress)]
    [StringLength(256)]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [InverseProperty(nameof(TheaterLibrary.Admin.User))]
    public Admin? Admin { get; set; }

    [InverseProperty(nameof(TheaterLibrary.Client.User))]
    public Client? Client { get; set; }

    [InverseProperty(nameof(TheaterLibrary.Manager.User))]
    public Manager? Manager { get; set; }
    
    [InverseProperty(nameof(Log.User))]
    public ICollection<Log> Logs { get; set; }
}