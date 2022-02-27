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

namespace LibApp.Controllers.Api
{
  [Route("api/[controller]")]
  [ApiController]
  public class BooksController : ControllerBase
  {
    public BooksController(ApplicationDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    // GET /api/customers
    [HttpGet]
    public IActionResult GetBooks()
    {
      var books = _context.Books
          .Include(b => b.Genre)
          .ToList();
      //   .Select(_mapper.Map<Book, BookDto>);

      return Ok(books);
    }

    private ApplicationDbContext _context;
    private IMapper _mapper;
  }
}
