var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultFiles(); // Enables index.html as default //Para ma connect ang HTML & C# :>
app.UseStaticFiles();  // Serves files from wwwroot // Use ani para maka Connect ang HTML & C# :> and also kani sad dapita kay para ma work ang both languages

app.UseAuthorization();


app.MapControllers();

app.Run();
