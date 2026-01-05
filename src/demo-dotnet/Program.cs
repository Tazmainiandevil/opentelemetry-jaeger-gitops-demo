using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var activitySource = new ActivitySource("demo-dotnet-sample");

app.MapGet("/", () =>
{
    using var activity = activitySource.StartActivity("handle-root");
    activity?.SetTag("demo", "true");
    return Results.Ok(new { message = "Hello from demo-dotnet", time = DateTime.UtcNow });
});

app.MapGet("/healthz", () => Results.Ok("ok"));

app.Run();
