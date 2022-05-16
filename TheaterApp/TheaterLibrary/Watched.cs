﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterLibrary;

public class Watched
{
    public int ClientId { get; set; }
    
    public int ShowId { get; set; }
    
    [ForeignKey(nameof(ClientId))]
    [InverseProperty(nameof(TheaterLibrary.Client.ShowsWatched))]
    public virtual Client? Client { get; set; }
    
    [ForeignKey(nameof(ShowId))]
    [InverseProperty(nameof(TheaterLibrary.Show.ClientsWatched))]
    public virtual Show? Show { get; set; }
}