using CollegeApp.Configurations;
using CollegeApp.Data;
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

builder.Services.AddScoped<IMyLogger, LogToMemory>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();