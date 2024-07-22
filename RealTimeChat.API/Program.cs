using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealTimeChat.API;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

LoadConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


void LoadConfiguration(IConfiguration configuration)
{
    Configuration.JwtKey = configuration.GetValue<string>("JwtKey")!;
    Configuration.ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
}

void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<RealTimeChatDbContext>(opt =>
    {
        opt.UseSqlServer(Configuration.ConnectionString);
    });


    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true
        };
    });
}