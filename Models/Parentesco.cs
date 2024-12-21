using System.Text.Json.Serialization;

namespace Desenhos.Models;

public class Parentesco
{
    public long Id { get; set; }
    public long PaiId { get; set; }
    public long FilhoId { get; set; }
    public Desenho? Pai { get; set; }
    public Desenho? Filho { get; set; }
}