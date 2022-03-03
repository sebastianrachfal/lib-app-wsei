using LibApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Interfaces;
using LibApp.Repositories;

namespace LibApp
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(
              Configuration.GetConnectionString("DefaultConnection")));
      services.AddDatabaseDeveloperPageExceptionFilter();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      services.AddDefaultIdentity<IdentityUser>(options =>
      {
        options.SignIn.RequireConfirmedAccount = false;
        options.ClaimsIdentity.RoleClaimType = "User";
      })
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();


      services.AddScoped<IBookRepository, BookRepository>();

      services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });


      Task.Run(() => this.CreateRoles(serviceProvider)).Wait();
    }


    private async Task CreateRoles(IServiceProvider serviceProvider)
    {
      var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

      foreach (string rol in this.Configuration.GetSection("Roles").Get<List<string>>())
      {
        if (!await roleManager.RoleExistsAsync(rol))
        {
          await roleManager.CreateAsync(new IdentityRole(rol));
        }
      }

      var userList = new List<Tuple<IdentityUser, string>>();

      userList.Add(
          new Tuple<IdentityUser, string>(new IdentityUser
          {
            UserName = "rachfal-user@sebastian.com",
            Email = "rachfal-user@sebastian.com",
          }, "User")
      );
      userList.Add(
          new Tuple<IdentityUser, string>(new IdentityUser
          {
            UserName = "rachfal-sm@sebastian.com",
            Email = "rachfal-sm@sebastian.com",

          }, "StoreManager")
      );
      userList.Add(
          new Tuple<IdentityUser, string>(new IdentityUser
          {
            UserName = "rachfal-owner@sebastian.com",
            Email = "rachfal-owner@sebastian.com",

          }, "Owner")
      );

      string userPWD = Configuration["UserPassword"];

      foreach (var user in userList)
      {
        var _user = await userManager.FindByEmailAsync(user.Item1.Email);

        if (_user == null)
        {
          var newUser = await userManager.CreateAsync(user.Item1, userPWD);
          if (newUser.Succeeded)
          {
            await userManager.AddToRoleAsync(user.Item1, user.Item2);
          }
        }
      }

    }
  }
}
