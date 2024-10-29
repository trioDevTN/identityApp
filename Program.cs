using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options=>
{
    options.UseInMemoryDatabase("InMemoryDb");
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Defining Identity Core Service
builder.Services.AddIdentityCore<User>(options=>
{
    //Password Configuration
    options.Password.RequiredLength=8;
    options.Password.RequireDigit=true;
    options.Password.RequireLowercase=true;
    options.Password.RequireUppercase=true;
    options.Password.RequireNonAlphanumeric=true;

    //For Email confirmation
    options.SignIn.RequireConfirmedEmail=true;

})
.AddRoles<IdentityRole>() // be able to add roles
.AddRoleManager<RoleManager<IdentityRole>>() // be able to make use of RoleManager
.AddEntityFrameworkStores<Context>() // Providing our context
.AddSignInManager<SignInManager<User>>() // make use of signin manager
.AddUserManager<UserManager<User>>() // make use of user manager to create users
.AddDefaultTokenProviders(); // be able to create tokens for email confirmation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
