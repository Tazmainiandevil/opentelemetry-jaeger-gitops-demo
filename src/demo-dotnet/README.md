# DemoApp (minimal)

This minimal ASP.NET Core app is used by the blog demo to show auto-instrumentation with OpenTelemetry.

Build & run locally:

```bash
cd src/demo-dotnet/
dotnet run
# then curl http://localhost:5000/
```

Publish for Docker (the repo `src/apps/demo-dotnet/Dockerfile` already publishes this project):

```bash
docker build -t your-registry/demo-dotnet:latest -f dev/demo-dotnet/Dockerfile .
docker push your-registry/demo-dotnet:latest
```

Endpoints:

- `GET /` — returns a JSON greeting and creates a short `Activity` to ensure traces are generated.
- `GET /healthz` — health check.
