using DatabaseConnection.Data;
using DatabaseConnection.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Depency Injectin of IConnectionServiceAsync
builder.Services.AddScoped<IConnectionServiceAsync, ConnectionServiceAsync>();

//Depency Injectin of IDaoEmpleadosAsync
builder.Services.AddScoped<IDaoEmpleadosAsync, DaoEmpleadosAsync>();



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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
