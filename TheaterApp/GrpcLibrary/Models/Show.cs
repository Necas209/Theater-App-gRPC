using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public sealed class Show
{
    public Show()
    {
        Name = "";
        Synopsis = "";
        Sessions = new HashSet<Session>();
        ClientsWatched = new HashSet<Watched>();
    }

    [Key] public int Id { get; init; }

    public string Name { get; set; }

    public string Synopsis { get; set; }

    public TimeSpan Length { get; set; }

    public int GenreId { get; init; }

    public Genre? Genre { get; set; }

    public ICollection<Session> Sessions { get; set; }

    public ICollection<Watched> ClientsWatched { get; set; }
}