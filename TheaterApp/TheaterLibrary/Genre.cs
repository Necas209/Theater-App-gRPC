using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public sealed class Genre
{
    public Genre()
    {
        Shows = new HashSet<Show>();
    }
    
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }

    [InverseProperty(nameof(Show.Genre))]
    public ICollection<Show> Shows { get; set; }
}