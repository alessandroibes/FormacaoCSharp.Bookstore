using System.ComponentModel.DataAnnotations;

namespace FormacaoCSharp.Bookstore.Communication.Requests;

public class RequestCreateBookJson
{
    [Required, MinLength(2), MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [Required, MinLength(2), MaxLength(120)]
    public string Author { get; set; } = string.Empty;

    [Required]
    public string Genre { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
