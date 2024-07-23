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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly));

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

    //Injetando dependências
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
    services.AddScoped<IMessageRepository, MessageRepository>();
    services.AddScoped<IRoomParticipantRepository, RoomParticipantRepository>();

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