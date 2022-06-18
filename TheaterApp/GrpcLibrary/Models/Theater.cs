using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Theater
{
    public Theater()
    {
        Name = "";
        Location = "";
        Address = "";
        Email = "";
        PhoneNumber = "";
        Sessions = new HashSet<Session>();
    }

    [Key] public int Id { get; init; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string Address { get; set; }

    [DataType(DataType.EmailAddress)] public string Email { get; set; }

    [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; }

    public ICollection<Session> Sessions { get; set; }
}