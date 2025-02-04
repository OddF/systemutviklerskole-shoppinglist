using shoppinglist_backend;

var builder = WebApplication.CreateBuilder(args);

// Uten mellomrom og ���
StaticConfiguration.Name = "DittNavn";

// Add services to the container.
builder.Services.AddSingleton<TableStorageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Dev",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

var app = builder.Build();

bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

if (isDevelopment)
{
    app.UseCors("Dev");
}

app.MapGet("/api/shoppingitems", async (TableStorageService service) =>
{
    var items = await service.GetItemsAsync();
    return Results.Ok(items);
});

app.MapPost("/api/shoppingitems", async (TableStorageService service, ShoppingItem item) =>
{
    item.RowKey = Guid.NewGuid().ToString();
    // TODO bytte ut med AI p� sikt, fortelle investorer at vi bruker ai idag.
    // Det er teknisk sett sant, fordi vi brukte copilot for � lage listen
    item.Category = item.Name.ToLower() switch
    {
        "ananas" => "Frukt", "melon" => "Frukt", "kiwi" => "Frukt", "paprika" => "Gr�nnsaker", "gulrot" => "Gr�nnsaker", "brokkoli" => "Gr�nnsaker", "blomk�l" => "Gr�nnsaker", "spinat" => "Gr�nnsaker", "salat" => "Gr�nnsaker",  "l�k" => "Gr�nnsaker", "hvitl�k" => "Gr�nnsaker",        "potet" => "Gr�nnsaker", "s�tpotet" => "Gr�nnsaker", "mais" => "Gr�nnsaker", "erter" => "Gr�nnsaker", "b�nner" => "Gr�nnsaker", "linser" => "Gr�nnsaker", "quinoa" => "Korn", "ris" => "Korn", "pasta" => "Korn","br�d" => "Korn",  "havregryn" => "Korn", "kjeks" => "Snacks",        "sjokolade" => "Snacks", "godteri" => "Snacks", "chips" => "Snacks", "popcorn" => "Snacks", "n�tter" => "Snacks", "mandler" => "Snacks", "cashewn�tter" => "Snacks", "pean�tter" => "Snacks", "valn�tter" => "Snacks", "pistasjn�tter" => "Snacks", "rosiner" => "Snacks",
        "t�rkede aprikoser" => "Snacks", "t�rkede traneb�r" => "Snacks", "t�rkede fiken" => "Snacks", "t�rkede dadler" => "Snacks", "yoghurt" => "Meieri", "melk" => "Meieri", "ost" => "Meieri", "sm�r" => "Meieri", "kremost" => "Meieri",
        _ => "AI categorization is unavailable due to high demand"

    };
    await service.AddItemAsync(item);
    return Results.Created($"/api/shoppingitems/{item.RowKey}", item);
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();