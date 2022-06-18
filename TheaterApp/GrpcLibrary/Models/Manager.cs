using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class Manager
{
    [Key] public int Id { get; set; }

    [ForeignKey(nameof(Id))] public User? User { get; set; }
}