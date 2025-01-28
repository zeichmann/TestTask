using TestTask.Application.CardActions.Queries;
using TestTask.Infrastructure.Abstraction.Services;
using TestTask.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CardActionsQuery).Assembly));

builder.Services.AddScoped<IActionProviderService, ActionProviderService>();
builder.Services.AddScoped<ICardService, CardService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();