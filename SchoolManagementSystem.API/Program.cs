using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Business.Interfaces;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.DataAccess;
using SchoolManagementSystem.DataAccess.Interfaces;
using SchoolManagementSystem.DataAccess.Repositories;
using SchoolManagementSystem.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SchoolDbContext>(
    options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolManagementConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<SchoolDbContext>();

// Repository registrations
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// Service registrations  
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
