using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation.AspNetCore;
using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Mapping;
using LW4._2_Kovalchuk.Models;
using LW4._2_Kovalchuk.Services;
using LW4._2_Kovalchuk.Validators;
using LW4._2_Kovalchuk.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using SortTest.Test;
using MongoAuthApi.Helpers;
var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(Int32.Parse(port));
});
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UserItemValidator>());
var dbClient = MobgoDBClient.Instance;

builder.Services.AddSingleton(dbClient.GetCollection<UserItem>("Users"));
builder.Services.AddSingleton(dbClient.GetCollection<BoardGameItem>("BoardGames"));
builder.Services.AddSingleton(dbClient.GetCollection<GameSessionItem>("GameSessions"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "¬вед≥ть токен у формат≥: Bearer {токен}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtTokenGenerator>();
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IBoardGamesRepository, BoardGamesRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IGameSessionsRepository, GameSessionsRepository>();
builder.Services.AddScoped<IValidator<BoardGameItem>, BoardGameItemValidator>();
builder.Services.AddScoped<IValidator<UserItem>, UserItemValidator>();
builder.Services.AddScoped<IValidator<GameSessionItem>, GameSessionValidator>();
builder.Services.AddScoped<IBoardGamesService, BoardGamesService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGameSessionsService, GameSessionsService>();
builder.Services.AddAutoMapper(typeof(UserProfile), typeof(BoardGameProfile), typeof(GameSessionProfile));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
