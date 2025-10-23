using FormacaoCSharp.Bookstore.Models;

namespace FormacaoCSharp.Bookstore.Repositories;

public interface IBookRepository
{
    IEnumerable<Book> GetAll(string? genre = null, string? author = null, string? title = null);
    Book? GetById(Guid id);
    void Add(Book book);
    void Update(Book book);
    void Remove(Guid id);
    bool ExistsByTitleAndAuthor(string title, string author, Guid? excludingId = null);
    IReadOnlyList<string> GetAllowedGenres();
}
