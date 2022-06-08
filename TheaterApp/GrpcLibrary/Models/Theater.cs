using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class Theater
{
    public Theater()
    {
        Sessions = new HashSet<Session>();
    }

    [Key] public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Address { get; set; } = null!;

    [DataType(DataType.EmailAddress)] public string Email { get; set; } = null!;

    [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; set; } = null!;

    [InverseProperty(nameof(Session.Theater))]
    public ICollection<Session> Sessions { get; set; }
}