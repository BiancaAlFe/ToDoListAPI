using System.Text.Json.Serialization;

namespace Desenhos.Models;

public class Desenho
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int Peso { get; set; }
    public ICollection<Parentesco> Filhos { get; } = new List<Parentesco>();
    public ICollection<Parentesco> Pais { get; } = new List<Parentesco>();
}