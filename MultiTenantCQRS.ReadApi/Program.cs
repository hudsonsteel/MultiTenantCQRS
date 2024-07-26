using MultiTenantCQRS.Read.Application.Configurations;
using MultiTenantCQRS.Read.Domain.Configurations;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.RabbitMq.Configurations;
using MultiTenantCQRS.Read.SqlServer.Configurations;
using MultiTenantCQRS.ReadApi.Configurations.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAplicationConfig();
builder.Services.AddDomainConfig(builder.Configuration);
builder.Services.AddRepositoriesConfig(builder.Configuration);
builder.Services.AddRabbitMqConfig(builder.Configuration);

builder.Services.AddHttpContextAccessor();

// Adicionar serviços ao contêiner
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new LowercaseControllerModelConvention());
});
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