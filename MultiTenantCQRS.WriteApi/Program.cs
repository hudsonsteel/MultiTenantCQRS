using MultiTenantCQRS.Write.Application.Configurations;
using MultiTenantCQRS.Write.Domain.Configurations;
using MultiTenantCQRS.Write.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Write.RabbitMq.Configurations.DependencyConfig;
using MultiTenantCQRS.Write.SqlServer.Configurations;
using MultiTenantCQRS.WriteApi.Configurations.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDomainConfig(builder.Configuration);
builder.Services.AddAplicationConfig();
builder.Services.AddRepositoriesConfig(builder.Configuration);
builder.Services.AddRabbitMqConfig(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options => { options.Conventions.Add(new LowercaseControllerModelConvention()); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize schemas and tables
using (var scope = app.Services.CreateScope())
{
    var schemaRepository = scope.ServiceProvider.GetRequiredService<ISchemaRepository>();
    await schemaRepository.InitializeAsync(CancellationToken.None);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();