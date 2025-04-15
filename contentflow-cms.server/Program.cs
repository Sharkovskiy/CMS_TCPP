using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using contentflow_cms.server.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ContentflowContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("contentflow_cmsserverContext") ?? throw new InvalidOperationException("Connection string 'contentflow_cmsserverContext' not found.")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
