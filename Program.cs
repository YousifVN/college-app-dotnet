using System.Text;
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.MyLogger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

# region loggin settings

Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Information()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Hour)
    .CreateLogger();

// use serilog (file) along with builtin logging providers (console, debug, etc..)
builder.Logging.AddSerilog();

// use serilog and clear other logging providers
// builder.Host.UseSerilog();

# endregion 

builder.Services.AddDbContext<CollegeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"));
});

// builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers( 
    // options => options.ReturnHttpNotAcceptable = true
    ).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();

#region swagger config

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT authorization header using the bearer scheme. Enter Bearer [space] add your token in the text input. Example: Bearer swe232jds",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "oauth2",
                
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

#endregion


builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<IMyLogger, LogToMemory>();

builder.Services.AddCors(options => options.AddPolicy("MyTestCores", policy =>
{
    // allow all origins
    // policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    
    // allow some origins
    policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
}));

#region jwt settings

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecret"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // make sure to make this true in production environment 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        // too lazy to check what the issuer and audience are for
        ValidateIssuer = false, 
        ValidateAudience = false,
    };
});

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyTestCores");

app.UseAuthorization();

app.MapControllers();

app.Run();