using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Theater
{
    [Key] public int Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    [DataType(DataType.EmailAddress)] public string Email { get; init; } = string.Empty;

    [DataType(DataType.PhoneNumber)] public string PhoneNumber { get; init; } = string.Empty;
}