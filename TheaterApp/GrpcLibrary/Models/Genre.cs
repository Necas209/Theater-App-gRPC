using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Genre
{
    [Key] public int Id { get; init; }

    public string Name { get; set; } = string.Empty;
}