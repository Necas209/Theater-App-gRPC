using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcLibrary.Models;

public sealed class Admin
{
    [Key] public int Id { get; set; }

    // ReSharper disable once UnusedMember.Global
    [ForeignKey(nameof(Id))] public User? User { get; set; }
}