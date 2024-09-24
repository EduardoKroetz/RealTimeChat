using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealTimeChat.API;
using RealTimeChat.Infrastructure.Persistence.Context;
using RealTimeChat.Application.Commands.CreateUser;
using System.Text;
using RealTimeChat.Core.Services;
using RealTimeChat.Infrastructure.Auth;
using RealTimeChat.Core.Repositories;
using RealTimeChat.Infrastructure.Persistence.Repositories;
using RealTimeChat.API.Middlewares;
using RealTimeChat.API.SignalR;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var databaseConnectionString = builder.Configuration.GetConnectionString("PostgreSQL") ?? throw new Exception("Invalid postgreSQL connection string");

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = 0;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly));

LoadConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();

// Aplica migrações automaticamente ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RealTimeChatDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.Urls.Add("http://*:8080");

app.Run();


void LoadConfiguration(IConfiguration configuration)
{
    Configuration.JwtKey = configuration.GetValue<string>("JwtKey")!;
    Configuration.ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
    Configuration.DefaultFrontendBaseUrl = configuration.GetValue<string>("DefaultFrontendBaseUrl")!; //configuration to add cors
}

void ConfigureServices(IServiceCollection services)
{

    services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
            builder =>
            {
                builder.WithOrigins(Configuration.DefaultFrontendBaseUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });

    services.AddDbContext<RealTimeChatDbContext>(opt =>
        {
        opt.UseNpgsql(databaseConnectionString);
    });

    services.AddExceptionHandler<GlobalExceptionHandler>();
    services.AddProblemDetails();

    services.AddLogging();

    //Injetando depend�ncias
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
    services.AddScoped<IMessageRepository, MessageRepository>();
    services.AddScoped<IRoomParticipantRepository, RoomParticipantRepository>();

    services.AddSignalR();

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

public class Startup { }