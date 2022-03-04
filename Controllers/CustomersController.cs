using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers
{
  [Authorize(Roles = "StoreManager, Owner")]
  public class CustomersController : Controller
  {
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
      _context = context;
    }

    public ViewResult Index()
    {
      return View();
    }

    public IActionResult Details(int id)
    {
      var customer = _context.Customers
          .Include(c => c.MembershipType)
          .SingleOrDefault(c => c.Id == id);

      if (customer == null)
      {
        return Content("User not found");
      }

      return View(customer);
    }
    [Authorize(Roles = "Owner")]
    public IActionResult New()
    {
      var membershipTypes = _context.MembershipTypes.ToList();

      var viewModel = new CustomerFormViewModel()
      {
        MembershipTypes = membershipTypes
      };

      return View("CustomerForm", viewModel);
    }

    [Authorize(Roles = "Owner")]
    public IActionResult Edit(int id)
    {
      var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
      if (customer == null)
      {
        return NotFound();
      }

      var viewModel = new CustomerFormViewModel(customer)
      {
        MembershipTypes = _context.MembershipTypes.ToList()
      };

      return View("CustomerForm", viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Owner")]
    [ValidateAntiForgeryToken]
    public IActionResult Save(Customer customer)
    {
      if (!ModelState.IsValid)
      {
        var viewModel = new CustomerFormViewModel(customer)
        {
          MembershipTypes = _context.MembershipTypes.ToList()
        };

        return View("CustomerForm", viewModel);
      }

      if (customer.Id == 0)
      {
        _context.Customers.Add(customer);
      }
      else
      {
        var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
        customerInDb.Name = customer.Name;
        customerInDb.Birthdate = customer.Birthdate;
        customerInDb.MembershipTypeId = customer.MembershipTypeId;
        customerInDb.HasNewsletterSubscribed = customer.HasNewsletterSubscribed;
      }

      try
      {
        _context.SaveChanges();
      }
      catch (DbUpdateException e)
      {
        Console.WriteLine(e);
      }

      return RedirectToAction("Index", "Customers");
    }
  }
}