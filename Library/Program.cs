using AutoMapper;
using FluentValidation.AspNetCore;

using Database;
using Database.Entities;
using Microsoft.AspNetCore.HttpLogging;
using Repositories;
using FluentValidation;
using Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryContext>(options =>
{
    options.UseLazyLoadingProxies().UseInMemoryDatabase(databaseName: "LibraryDatabase");
});

builder.Services.AddScoped<IGenericRepository<Book>, GenericRepository<Book>>();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestMethod
                            | HttpLoggingFields.RequestHeaders
                            | HttpLoggingFields.RequestQuery
                            | HttpLoggingFields.RequestBody;

});
builder.Services.AddSingleton(new MapperConfiguration(ctg => {
    ctg.AddProfile<BookProfile>();
    ctg.AddProfile<ReviewProfile>();
    ctg.AddProfile<RatingProfile>();
    ctg.AddProfile<IdContaningProfile>();
}).CreateMapper());

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "CORSOpenPolicy", 
        builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()
    );
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CORSOpenPolicy");

app.Run();
