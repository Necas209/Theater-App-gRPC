using System.ComponentModel.DataAnnotations;

namespace GrpcLibrary.Models;

public class Log
{
    public Log()
    {
        Message = "";
        Stamp = DateTime.Now;
    }

    [Key] public int Id { get; set; }

    public DateTime Stamp { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; }

    public User? User { get; set; }
}