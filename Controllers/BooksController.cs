using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using LibApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers
{
  public class BooksController : Controller
  {
    private readonly IBookRepository repository;

    public BooksController(IBookRepository _repo)
    {
      repository = _repo;
    }

    public IActionResult Index()
    {
      var books = repository.GetBooks().ToList();

      return View(books);
    }

    public IActionResult Details(int id)
    {
      var book = repository.GetBooks()
          .SingleOrDefault(b => b.Id == id);

      return View(book);
    }

    [Authorize(Roles = "StoreManager, Owner")]
    public IActionResult Edit(int id)
    {
      var book = repository.GetBookById(id);
      if (book == null)
      {
        return NotFound();
      }

      var viewModel = new BookFormViewModel
      {
        Book = book,
        Genres = repository.GetGenres().ToList()
      };

      return View("BookForm", viewModel);
    }

    [Authorize(Roles = "StoreManager, Owner")]
    public IActionResult New()
    {
      var viewModel = new BookFormViewModel
      {
        Genres = repository.GetGenres().ToList()
      };

      return View("BookForm", viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "StoreManager, Owner")]
    public IActionResult Save(Book book)
    {
      if (!ModelState.IsValid) return RedirectToAction("New", "Books");

      if (book.Id == 0)
      {
        book.DateAdded = DateTime.Now;
        repository.AddBook(book);
      }
      else
      {
        var bookInDb = repository.GetBookById(book.Id);

        bookInDb.Name = book.Name;
        bookInDb.AuthorName = book.AuthorName;
        bookInDb.GenreId = book.GenreId;
        bookInDb.NumberInStock = book.NumberInStock;
        bookInDb.ReleaseDate = book.ReleaseDate;
        bookInDb.DateAdded = book.DateAdded;
      }

      try
      {
        repository.Save();
      }
      catch (DbUpdateException e)
      {
        Console.WriteLine(e);
      }

      return RedirectToAction("Index", "Books");
    }
  }
}
