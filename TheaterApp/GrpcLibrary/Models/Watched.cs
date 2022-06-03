using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public class Watched
{ 
    public int ClientId { get; set; }
    
    public int ShowId { get; set; }
    
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(GrpcLibrary.Models.Client.ShowsWatched))]
    public virtual Client? Client { get; set; }

    [ForeignKey(nameof(ShowId))]
    [InverseProperty(nameof(GrpcLibrary.Models.Show.ClientsWatched))]
    public virtual Show? Show { get; set; }
}