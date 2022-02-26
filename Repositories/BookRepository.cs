using LibApp.Interfaces;
using LibApp.Data;
using LibApp.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Repositories
{
  public class BookRepository : IBookRepository
  {
    private readonly ApplicationDbContext context;

    public BookRepository(ApplicationDbContext _context)
    {
      context = _context;
    }

    public IEnumerable<Book> GetBooks()
    {
      return context.Books.Include(b => b.Genre);
    }

    public Book GetBookById(int bookId)
    {
      return context.Books.Find(bookId);
    }

    public IEnumerable<Genre> GetGenres()
    {
      return context.Genre;
    }

    public void AddBook(Book book)
    {
      context.Books.Add(book);
    }

    public void UpdateBook(Book book)
    {
      context.Books.Update(book);
    }

    public void DeleteBook(int bookId)
    {
      context.Books.Remove(GetBookById(bookId));
    }

    public void Save()
    {
      context.SaveChanges();
    }
  }
}