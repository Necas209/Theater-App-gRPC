using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public sealed class Show
{
    public Show()
    {
        Sessions = new HashSet<Session>();
        ClientsWatched = new HashSet<Watched>();
    }
    
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Synopsis { get; set; }
    
    public int GenreId { get; set; }
    
    [ForeignKey(nameof(GenreId))]
    [InverseProperty(nameof(TheaterLibrary.Genre.Shows))]
    public Genre Genre { get; set; }
    
    [InverseProperty(nameof(Session.Show))]
    public ICollection<Session> Sessions { get; set; }
    
    [InverseProperty(nameof(Watched.Show))]
    public ICollection<Watched> ClientsWatched { get; set; }
}