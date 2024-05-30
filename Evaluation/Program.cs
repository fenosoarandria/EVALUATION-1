using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Liaison base de données
builder.Services.AddDbContext<ApplicationDbContext>
    (options =>options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext")));

//Ajouter repository
builder.Services.AddScoped<AdministrateurRepository>();
builder.Services.AddScoped<UniteRepository>();
builder.Services.AddScoped<TravauxRepository>();
builder.Services.AddScoped<TypeMaisonRepository>();
builder.Services.AddScoped<TravauxTypeMaisonRepository>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<DevisRepository>();
builder.Services.AddScoped<TypeFinitionRepository>();
builder.Services.AddScoped<HistoriqueDevisTravauxRepository>();
builder.Services.AddScoped<HistoriqueDevisFinitionRepository>();
builder.Services.AddScoped<PayementRepository>();
builder.Services.AddScoped<LieuRepository>();

//Login
builder.Services.AddScoped<LoginRepository>();
//Session filter
builder.Services.AddScoped<SessionVerificationFilter>();
//import
builder.Services.AddScoped<Import>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1000); // Réglez la durée d'expiration de la session selon vos besoins.
});

// Configure the HTTP request pipeline.
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Middleware pour gérer les cookies et la session
app.UseCookiePolicy();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Connexion}/{action=Login}/{id?}");

app.Run();
