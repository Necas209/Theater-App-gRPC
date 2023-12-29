namespace GrpcLibrary.Models;

public sealed class Watched
{
    public int ClientId { get; init; }

    public int ShowId { get; init; }

    public Client? Client { get; init; }

    public Show? Show { get; init; }
}