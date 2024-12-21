using System.Text.Json.Serialization;

namespace Desenhos.Models;

public class ParentescoDTO
{
    public long Id { get; set; }
    public long PaiId { get; set; }
    public long FilhoId { get; set; }
}