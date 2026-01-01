using GestionBibliotheque.Services; 


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Ajouter le cache en mémoire pour la session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // durée de vie de la session
    options.Cookie.HttpOnly = true;                  // protège le cookie côté client
    options.Cookie.IsEssential = true;              // nécessaire pour fonctionner même si l'utilisateur refuse les cookies facultatifs
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();

// **Forcer la page par défaut sur /Login**
app.MapGet("/", context =>
{
	context.Response.Redirect("/login");
	return Task.CompletedTask;
});
app.Run();
