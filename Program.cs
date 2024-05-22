
using CQRSWithDocker_Identity.Data;
using CQRSWithDocker_Identity.EncryptionLayer;
using CQRSWithDocker_Identity.Features.Loging_system.Commands.Create;
using CQRSWithDocker_Identity.Features.Users.Commands.Create;
using CQRSWithDocker_Identity.Features.Users.Commands.Delete;
using CQRSWithDocker_Identity.Features.Users.Queries.Get;
using CQRSWithDocker_Identity.Features.Users.Queries.Lists;
using CQRSWithDocker_Identity.Logger;
using MediatR;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;

});
builder.Services.AddW3CLogging(logging =>
{
    // Log all W3C fields
    logging.LoggingFields = W3CLoggingFields.All;

    logging.AdditionalRequestHeaders.Add("x-forwarded-for");
    logging.AdditionalRequestHeaders.Add("x-client-ssl-protocol");
    logging.FileSizeLimit = 5 * 1024 * 1024;
    logging.RetainedFileCountLimit = 100;
    logging.FileName = "MyLogFile";
    logging.LogDirectory = "./logger/logs";
    logging.FlushInterval = TimeSpan.FromSeconds(2);
});
builder.Services.AddHttpLoggingInterceptor<Logger>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }

    );
builder.Services.AddDbContext<DataContext>(db => db.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<DataContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseStaticFiles();

app.UseHttpLogging();
app.MapControllers();
app.MapGet("/Users/{id:guid}", async (ILogger<Program> logger,  Guid id, ISender mediatr) =>
{
    logger.LogInformation("/Users/id", id);
   
    var product = await mediatr.Send(new GetUserQuery(id));
    if (product == null) return Results.NotFound();   
    return Results.Ok(product);
})
.RequireAuthorization().WithHttpLogging(HttpLoggingFields.ResponsePropertiesAndHeaders); ;

app.MapGet("/Users", async (ISender mediatr) =>
{
    var products = await mediatr.Send(new UserListQuery());    
    return Results.Ok(products);
}).RequireAuthorization().WithHttpLogging(HttpLoggingFields.ResponsePropertiesAndHeaders); ;

app.MapPost("/Users", async (CreateUserCommand command, ISender mediatr) =>
{
    var productId = await mediatr.Send(command);
    if (Guid.Empty == productId) return Results.BadRequest();
    return Results.Created($"/Users/{productId}", new { id = productId });
}).RequireAuthorization().WithHttpLogging(HttpLoggingFields.ResponsePropertiesAndHeaders); ;

app.MapDelete("/Users/{id:guid}", async (Guid id, ISender mediatr) =>
{
    await mediatr.Send(new DeleteUserCommand(id));
    return Results.NoContent();
}).RequireAuthorization().WithHttpLogging(HttpLoggingFields.ResponsePropertiesAndHeaders); ;
app.Logger.LogInformation("Starting the app");
app.UseW3CLogging();
//app.UseMiddleware<EncryptionMiddleware>();
app.Run();
