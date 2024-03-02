using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ToDoDbContext>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder => builder.AllowAnyOrigin()
//                         .AllowAnyMethod()
//                         .AllowAnyHeader());
// });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/items", async (ToDoDbContext dbContext) =>
{
     var items = await dbContext.Items.ToListAsync();
     if(items!=null)
    return items;
    return [];
});
app.MapPost("/items", async (Item item, ToDoDbContext dbContext) =>
{
    await dbContext.Items.AddAsync(item);
    await dbContext.SaveChangesAsync();
    return item;
});
app.MapPut("/items/{id}", async (Item item, int id, ToDoDbContext dbContext) =>
{
    var i = await dbContext.Items.FindAsync(id);
    if (i == null)
        return Results.BadRequest("there is no such item!!!");

    i.Name = item.Name;
    i.IsComplete = item.IsComplete;

    await dbContext.SaveChangesAsync();
    return Results.Created($"/", i);
});
app.MapDelete("/items/{id}", async (int id, ToDoDbContext dbContext) =>
{
    var i = await dbContext.Items.FindAsync(id);
    if (i != null)
    {
        dbContext.Items.Remove(i);
        await dbContext.SaveChangesAsync();
    }
});

// app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" }, 
//                           () => "This is an options or head request ");

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.Run();