var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

List<Item> items = new List<Item>
{
    new Item { Id = 0, Name = "Laptop", Description = "Gaming laptop", Price = 1200, Quantity = 5 },
    new Item { Id = 1, Name = "Phone", Description = "Smartphone with OLED display", Price = 800, Quantity = 10 },
    new Item { Id = 2, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 200, Quantity = 15 }
};

app.MapGet("/", () => "Welcome to the Simple Web API!");
app.MapGet("/items", () => items);
app.MapGet("/items/{id:int:min(0)}", (int id) =>
{
    if (id >= items.Count)
    {
        return Results.NotFound($"Item with ID {id} not found.");
    }
    else
    {
        return Results.Ok(items[id]);
    }
});
app.MapPost("/items", (Item newItem) =>
{
    newItem.Id = items.Count;
    items.Add(newItem);
    return Results.Created($"/items/{newItem.Id}", newItem);
});
app.MapPut("/items/{id:int:min(0)}", (int id, Item updatedItem) =>
{
    if (id >= items.Count)
    {
        return Results.NotFound($"Item with ID {id} not found.");
    }
    else
    {
        updatedItem.Id = id;
        items[id] = updatedItem;
        return Results.Ok(updatedItem);
    }
});
app.MapDelete("/items/{id:int:min(0)}", (int id) =>
{
    if (id >= items.Count)
    {
        return Results.NotFound($"Item with ID {id} not found.");
    }
    else
    {
        items.RemoveAt(id);
        return Results.Ok($"Item {id} deleted.");
    }
});

app.Run();
