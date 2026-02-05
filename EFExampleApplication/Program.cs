using EFExampleApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Environment)
    .AddSwagger()
    .AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
