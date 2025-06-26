using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Security;
using ClinicaMedicaAPIREST.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region  Authentication Configuration


	var key = builder.Configuration["Jwt:Key"];
	var securityKey = Encoding.UTF8.GetBytes(key!);

	builder.Services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

		}

	).AddJwtBearer(options =>

		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;

			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,

				IssuerSigningKey = new SymmetricSecurityKey(securityKey),
				ClockSkew = TimeSpan.Zero

			};

		}
	);
	



#endregion



// Add services to the container.
builder.Services.AddControllers();
// Add the DbConnectionService as a IDbConnectionService implementation
builder.Services.AddScoped<IDbConnectionService, DbConnectionService>();
builder.Services.AddScoped<daoPacientes>();
builder.Services.AddScoped<daoMedicos>();
builder.Services.AddScoped<daoCitas>();
builder.Services.AddScoped<daoUsuarios>();
builder.Services.AddScoped<daoEspecialidades>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();
