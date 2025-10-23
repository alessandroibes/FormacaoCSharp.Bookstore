using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FormacaoCSharp.Bookstore.Communication.Requests;

public class RequestUpdateBookJson
{
    [MinLength(2), MaxLength(120)]
    public string? Title { get; set; }

    [MinLength(2), MaxLength(120)]
    public string? Author { get; set; }

    public string? Genre { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; set; }
}
