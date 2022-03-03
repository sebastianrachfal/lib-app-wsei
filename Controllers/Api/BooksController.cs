using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers.Api
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class BooksController : ControllerBase
  {
    private readonly IBookRepository repository;
    public BooksController(IBookRepository _repo)
    {
      repository = _repo;
    }

    // GET /api/books
    [HttpGet]
    public IActionResult GetBooks()
    {
      var books = repository.GetBooks()
          .ToList();

      return Ok(books);
    }

    [Authorize(Roles = "StoreManager, Owner")]
    [HttpDelete("{id}")]
    public IActionResult RemoveBook(int id)
    {
      try
      {
        repository.DeleteBook(id);
        repository.Save();
      }
      catch (Exception)
      {
        return NotFound();
      }
      return Ok();
    }
  }
}
