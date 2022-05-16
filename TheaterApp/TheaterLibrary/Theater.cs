using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public sealed class Theater
{
    public Theater()
    {
        Sessions = new HashSet<Session>();
    }
    
    [Key] 
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Location { get; set; }
    
    public string Address { get; set; }
    
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    
    [InverseProperty(nameof(Session.Theater))]
    public ICollection<Session> Sessions { get; set; }
}