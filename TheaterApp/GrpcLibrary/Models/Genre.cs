using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Genre
{
    public Genre()
    {
        Name = "";
        Shows = new HashSet<Show>();
    }

    [Key] public int Id { get; init; }

    public string Name { get; set; }

    public ICollection<Show> Shows { get; set; }
}