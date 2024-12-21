using System.Text.Json.Serialization;

namespace Desenhos.Models;

public class DesenhoDTO
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int Peso { get; set; }
}