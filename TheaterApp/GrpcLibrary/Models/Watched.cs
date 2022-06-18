namespace GrpcLibrary.Models;

public sealed class Watched
{
    public int ClientId { get; init; }

    public int ShowId { get; init; }

    public Client? Client { get; set; }

    public Show? Show { get; set; }
}