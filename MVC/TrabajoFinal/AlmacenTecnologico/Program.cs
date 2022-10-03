using AlmacenTecnologico.Context;
using AlmacenTecnologico.Services;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add authentication and cookie services 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //ruta de login
    option.LoginPath = "/Access/Index";

    //tiempo maximo de sesion iniciada
    option.ExpireTimeSpan = TimeSpan.FromMinutes(50);

    //si no tiene acceso, lo devuelve al index
    option.AccessDeniedPath = "/Home/Error";
});

//adding connection string
builder.Services.AddSqlServer<TrabajoFinalContext>(builder.Configuration.GetConnectionString("TrabajoFinal"));

// adding scoped services
builder.Services.AddScoped<IFabricantesServces, FabricanteService>();
builder.Services.AddScoped<ITipoProductoService, TipoProductoService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

//
builder.Services.AddHttpContextAccessor();
// agrego el cliente http que solicitara peticiones a mi api de auth
builder.Services.AddHttpClient("ApiAuth", httpClient =>
{
    //TODO utilizar el puerto localhost que utilice la api de autorizacion al momento de ejecutarla
    httpClient.BaseAddress = new Uri("https://localhost:7206/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
