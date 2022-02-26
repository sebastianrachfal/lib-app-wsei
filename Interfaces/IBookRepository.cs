using System.Collections.Generic;
using LibApp.Models;

namespace LibApp.Interfaces
{
  public interface IBookRepository
  {
    IEnumerable<Book> GetBooks();
    IEnumerable<Genre> GetGenres();
    Book GetBookById(int bookId);
    void AddBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(int bookId);
    void Save();
  }

}