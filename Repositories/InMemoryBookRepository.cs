using FormacaoCSharp.Bookstore.Models;

namespace FormacaoCSharp.Bookstore.Repositories;

public class InMemoryBookRepository : IBookRepository
{
    private readonly List<Book> _books = new();

    private readonly List<string> _allowedGenres = new List<string>
    {
        "ficção",
        "romance",
        "mistério",
        "fantasia",
        "biografia",
        "tecnologia"
    };

    public IReadOnlyList<string> GetAllowedGenres() => _allowedGenres.AsReadOnly();

    public IEnumerable<Book> GetAll(string? genre = null, string? author = null, string? title = null)
    {
        var query = _books.AsQueryable();
        if (!string.IsNullOrWhiteSpace(genre)) query = query.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(author)) query = query.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(title)) query = query.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        return query.ToList();
    }

    public Book? GetById(Guid id) => _books.FirstOrDefault(b => b.Id == id);

    public void Add(Book book)
    {
        // timestamps
        book.CreatedAt = DateTime.UtcNow;
        book.UpdatedAt = book.CreatedAt;
        _books.Add(book);
    }

    public void Update(Book book)
    {
        var existing = GetById(book.Id);
        if (existing == null) return;
        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.Genre = book.Genre;
        existing.Price = book.Price;
        existing.Stock = book.Stock;
        existing.UpdatedAt = DateTime.UtcNow;
    }

    public void Remove(Guid id)
    {
        var existing = GetById(id);
        if (existing != null) _books.Remove(existing);
    }

    public bool ExistsByTitleAndAuthor(string title, string author, Guid? excludingId = null)
    {
        return _books.Any(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase)
        && b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)
        && (!excludingId.HasValue || b.Id != excludingId.Value));
    }
}
