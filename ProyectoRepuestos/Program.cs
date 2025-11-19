using ProyectoRepuestos;
using ProyectoRepuestos.Services;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();
var app = builder.Build();
await ApplicationDbContext.SeedRoles(app);
app.SetupMiddleware().Run();