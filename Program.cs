using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.MyLogger;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();

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