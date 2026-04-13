using SlimmeVoetbalclub.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Hier vertellen we de computer: "Hey, als de website straks de LedenRepository nodig heeft, dan mag hij die hier komen halen."
builder.Services.AddScoped<LedenRepository>();


// Zorgt dat de website overweg kan met de MVC-structuur (Models, Views, Controllers)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Basis-instellingen voor de website (zoals veiligheid en foutmeldingen)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default pagina is de Home Pagina
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();