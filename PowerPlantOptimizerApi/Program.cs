using PowerPlantOptimizerApi.Application;
using PowerPlantOptimizerApi.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost(
        "/productionplan", (LoadRequest request) =>
        {
            var result = Optimizer.Calculate(request.Fuels, request.Load, request.Powerplants);
            return Results.Ok(result);
        })
    .WithName("CalculateProductionPlan");

app.Run();