using AppStore.Models.Context;
using AppStore.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AppStore.Repositories.Abstract;
using AppStore.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database conection
builder.Services.AddDbContext<DatabaseContext>(opt => {
    // Log SQL queries to the console
    opt
        .LogTo(Console.WriteLine, new [] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
        .EnableSensitiveDataLogging();
    // Initialize the database connection using the connection string from appsettings.json
    opt
        .UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));
});

// Add Identity services
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(
        // options => {
        //     options.Password.RequireDigit = true;
        //     options.Password.RequiredLength = 6;
        // }
    )
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

// Agregar servicios de base de datos
builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

//  authentication application service
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

// Agregar servicio para subir archivos
builder.Services.AddScoped<IFileService, FileService>();


// ------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configurar la aplicacion.
app.UseHttpsRedirection();
app.UseRouting();

// Agregar autenticacion y autorizacion.
app.UseAuthentication();
app.UseAuthorization();

// Configurar las rutas y los activos estaticos.
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Cargar los datos de prueba en la base de datos.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;

        var context  = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();
        LoadDatabase.InsertarData(context, userManager, roleManager).Wait();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error durante la carga de datos de prueba en la base de datos.");
    }
}

// Ejecutar la aplicacion.
app.Run();
