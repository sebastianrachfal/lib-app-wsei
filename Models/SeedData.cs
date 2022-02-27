using System;
using System.Linq;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibApp.Models
{
  public static class SeedData
  {
    public static void Initialize(IServiceProvider serviceProvider)
    {
      SeedGenres(serviceProvider);
      SeedBooks(serviceProvider);
      SeedMembershipTypes(serviceProvider);
      SeedCustomers(serviceProvider);
    }

    public static void SeedBooks(IServiceProvider serviceProvider)
    {
      using (var context = new ApplicationDbContext(
          serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
      {
        if (context.Books.Any())
        {
          Console.WriteLine("[Books] Database already seeded");
          return;
        }

        context.Books.AddRange(
            new Book
            {
              Name = "Book name 1",
              AuthorName = "A. Mickiewicz",
              DateAdded = DateTime.Now,
              ReleaseDate = DateTime.Today,
              NumberInStock = 10,
              NumberAvailable = 10,
              Genre = context.Genre.Single(g => g.Id == 1),
              GenreId = 1,
            },
            new Book
            {
              Name = "Book name 2",
              AuthorName = "A. Mickiewicz",
              DateAdded = DateTime.Now,
              ReleaseDate = DateTime.Today,
              NumberInStock = 10,
              NumberAvailable = 10,
              Genre = context.Genre.Single(g => g.Id == 2),
              GenreId = 2,
            },
            new Book
            {
              Name = "Book name 3",
              AuthorName = "A. Mickiewicz",
              DateAdded = DateTime.Now,
              ReleaseDate = DateTime.Today,
              NumberInStock = 10,
              NumberAvailable = 10,
              Genre = context.Genre.Single(g => g.Id == 3),
              GenreId = 3,
            });

        context.SaveChanges();

        Console.WriteLine("[Books] Database has been seeded");
      }
    }
    public static void SeedGenres(IServiceProvider serviceProvider)
    {
      using (var context = new ApplicationDbContext(
          serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
      {
        if (context.Genre.Any())
        {
          Console.WriteLine("[Genres] Database already seeded");
          return;
        }

        context.Genre.AddRange(
            new Genre
            {
              Id = 1,
              Name = "Fantasy",
            },
            new Genre
            {
              Id = 2,
              Name = "Documentary",
            },
            new Genre
            {
              Id = 3,
              Name = "Romantic",
            });

        context.SaveChanges();

        Console.WriteLine("[Genres] Database has been seeded");
      }
    }
    public static void SeedCustomers(IServiceProvider serviceProvider)
    {
      using (var context = new ApplicationDbContext(
          serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
      {
        if (context.Customers.Any())
        {
          Console.WriteLine("[Customers] Database already seeded");
          return;
        }

        context.Customers.AddRange(
            new Customer
            {
              Name = "Adam Kowalski",
              HasNewsletterSubscribed = false,
              Birthdate = DateTime.Today,
              MembershipType = context.MembershipTypes.Single(mt => mt.Id == 1),
              MembershipTypeId = 1,
            },
            new Customer
            {
              Name = "Juliusz Kowalski",
              HasNewsletterSubscribed = false,
              Birthdate = DateTime.Today,
              MembershipType = context.MembershipTypes.Single(mt => mt.Id == 2),
              MembershipTypeId = 2,
            },
            new Customer
            {
              Name = "Wladymir Kowalski",
              HasNewsletterSubscribed = false,
              Birthdate = DateTime.Today,
              MembershipType = context.MembershipTypes.Single(mt => mt.Id == 3),
              MembershipTypeId = 3,
            });

        context.SaveChanges();

        Console.WriteLine("[Customers] Database has been seeded");
      }
    }

    public static void SeedMembershipTypes(IServiceProvider serviceProvider)
    {
      using (var context = new ApplicationDbContext(
          serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
      {
        if (context.MembershipTypes.Any())
        {
          Console.WriteLine("[MembershipTypes] Database already seeded");
          return;
        }

        context.MembershipTypes.AddRange(
            new MembershipType
            {
              Id = 1,
              Name = "Pay as You Go",
              SignUpFee = 0,
              DurationInMonths = 0,
              DiscountRate = 0
            },
            new MembershipType
            {
              Id = 2,
              Name = "Monthly",
              SignUpFee = 30,
              DurationInMonths = 1,
              DiscountRate = 10
            },
            new MembershipType
            {
              Id = 3,
              Name = "Quaterly",
              SignUpFee = 90,
              DurationInMonths = 3,
              DiscountRate = 15
            },
            new MembershipType
            {
              Id = 4,
              Name = "Yearly",
              SignUpFee = 300,
              DurationInMonths = 12,
              DiscountRate = 20
            });
        context.SaveChanges();

        Console.WriteLine("[MembershipTypes] Database has been seeded");
      }
    }
  }
}