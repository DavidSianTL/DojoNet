using calculatorService;
using ExamenUno.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CalculatorSoapClient>(_ =>
{
    var endpoint = new CalculatorSoapClient.EndpointConfiguration();
    return new CalculatorSoapClient(endpoint);
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ICalculatorService, CalculatorService>(); //este no me funcionó no me dejaba ni arrancar
builder.Services.AddHttpClient<IFakeStoreAPIService, FakeStoreAPIService>();
//builder.Services.AddHttpClient<ICalculatorService, CalculatorService>();

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
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
