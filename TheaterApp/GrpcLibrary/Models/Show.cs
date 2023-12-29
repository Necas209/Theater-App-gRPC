using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Show
{
    [Key] public int Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public string Synopsis { get; set; } = string.Empty;

    public TimeSpan Length { get; set; }

    public int GenreId { get; init; }

    public Genre? Genre { get; init; }
}