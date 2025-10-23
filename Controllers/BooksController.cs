using FormacaoCSharp.Bookstore.Communication.Requests;
using FormacaoCSharp.Bookstore.Models;
using FormacaoCSharp.Bookstore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FormacaoCSharp.Bookstore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _repo;

    public BooksController(IBookRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Cria um novo livro.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Create([FromBody] RequestCreateBookJson request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var allowed = _repo.GetAllowedGenres();

        if (!allowed.Any(g => g.Equals(request.Genre, StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new { error = "Gênero inválido", allowedGenres = allowed });

        if (_repo.ExistsByTitleAndAuthor(request.Title.Trim(), request.Author.Trim()))
            return Conflict(new { error = "Livro com mesmo título e autor já existe" });

        var book = new Book
        {
            Title = request.Title.Trim(),
            Author = request.Author.Trim(),
            Genre = request.Genre.Trim(),
            Price = request.Price,
            Stock = request.Stock
        };

        _repo.Add(book);

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    /// <summary>
    /// Lista todos os livros (filtros opcionais: genre, author, title)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Book>), StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] string? genre, [FromQuery] string? author, [FromQuery] string? title)
    {
        var books = _repo.GetAll(genre, author, title);
        return Ok(books);
    }

    /// <summary>
    /// Busca um livro pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var book = _repo.GetById(id);
        if (book == null) return NotFound(new { error = "Livro não encontrado" });
        return Ok(book);
    }

    /// <summary>
    /// Atualiza um livro existente
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult Update(Guid id, [FromBody] RequestUpdateBookJson request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existing = _repo.GetById(id);
        if (existing == null)
            return NotFound(new { error = "Livro não encontrado" });

        var allowed = _repo.GetAllowedGenres();
        if (!string.IsNullOrEmpty(request.Genre?.Trim())
            && !allowed.Any(g => g.Equals(request.Genre, StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new { error = "Gênero inválido", allowedGenres = allowed });

        if (!string.IsNullOrEmpty(request.Title?.Trim())
            && !string.IsNullOrEmpty(request.Author?.Trim())
            && _repo.ExistsByTitleAndAuthor(request.Title.Trim(), request.Author.Trim(), excludingId: id))
            return Conflict(new { error = "Outro livro com mesmo título e autor já existe" });

        if (!string.IsNullOrEmpty(request.Title?.Trim())) { existing.Title = request.Title.Trim(); }
        if (!string.IsNullOrEmpty(request.Author?.Trim())) { existing.Author = request.Author.Trim(); }
        if (!string.IsNullOrEmpty(request.Genre?.Trim())) { existing.Genre = request.Genre.Trim(); }
        if (request.Price.HasValue) { existing.Price = request.Price.Value; }
        if (request.Stock.HasValue) { existing.Stock = request.Stock.Value; }
        existing.UpdatedAt = DateTime.UtcNow;

        _repo.Update(existing);

        return NoContent();
    }

    /// <summary>
    /// Exclui um livro pelo ID
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var existing = _repo.GetById(id);
        if (existing == null)
            return NotFound(new { error = "Livro não encontrado" });

        _repo.Remove(id);
        return NoContent();
    }
}
