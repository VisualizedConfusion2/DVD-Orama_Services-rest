using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Middleware;
using DVD_Orama_Services_rest.Repos;
using DVD_Orama_Services_rest.Repos.Interfaces;
using DVD_Orama_Services_rest.Models.Entities;
using DVD_Orama_Services_rest.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DVD_Orama_Services_rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Firebase Admin SDK — optional; token verification is skipped if the file is missing
            try
            {
                var firebaseCredPath = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS")
                                       ?? "firebase-service-account.json";
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(firebaseCredPath)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Firebase Admin not initialised: {ex.Message}");
            }

            // Controllers
            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAll", policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
            });


            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // DI
            builder.Services.AddScoped<IMovieRepo, MovieRepo>();
            builder.Services.AddScoped<IMovieCollectionRepo, MovieCollectionRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IGenreRepo, GenreRepo>();
            builder.Services.AddScoped<IStreamingServiceRepo, StreamingServiceRepo>();

            // JWT Authentication
            var jwtKey = builder.Configuration["Jwt:Key"]!;
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddEndpointsApiExplorer();

            // Swagger with JWT support
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DVD-Orama API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}
            app.UseCors("AllowAll");

            app.UseMiddleware<FirebaseAuthMiddleware>();
            app.UseAuthentication(); // Must come before UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}