//The main entry point for the application.
var builder = WebApplication.CreateBuilder(args);

// Clear all logging providers and add Serilog.
builder.Logging.ClearProviders();
builder.Host.Serilog();

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger to the service collection.
builder.Services.AddSwagger(builder);

// Add CORS to the service collection.
builder.Services.AddCors();

//Dependency Injection Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

//Validators
builder.Services.AddValidatorsServices();

// Add API explorer endpoints to the service collection. see href="https://aka.ms/aspnetcore/swashbuckle"
builder.Services.AddEndpointsApiExplorer();

// Add rate limiting to the service collection.
builder.Services.AddRatelimiting(builder.Configuration);

var app = builder.Build();

// Use the developer exception page middleware.
app.UseDeveloperExceptionPage();

// Use the Swagger middleware.
app.UseSwagger();

// Use the custom Swagger UI middleware.
app.UseCustomSwaggerUI(app);

// Use the exception middleware.
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline to use HTTPS redirection, authentication,
// routing, authorization, rate limiting, endpoints, static files, and custom health checks UI.
app.UseHttpsRedirection()
   .UseAuthentication()
   .UseRouting()
   .UseAuthorization()
   .UseRateLimiter()
   .UseEndpoints(endpoints =>
   {
       endpoints.MapControllers();
   })
   .UseStaticFiles();

// Run the application.
await app.RunAsync();

/// <summary>
/// The main Program class.
/// </summary>
public partial class Program { }
