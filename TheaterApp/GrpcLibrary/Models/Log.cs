using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public class Log
{
    [Key] public int Id { get; init; }

    public DateTime Stamp { get; init; } = DateTime.Now;

    public int UserId { get; init; }

    public string Message { get; init; } = string.Empty;

    public User? User { get; init; }
}