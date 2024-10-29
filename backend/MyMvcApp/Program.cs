var builder = WebApplication.CreateBuilder(args);

// ����� ���� �� Startup
var startup = new MyMvcApp.Startup(builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MyMvcApp.Startup>>());

// ����� �������� ���� Startup.cs
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// ����� �-middleware ���� Startup.cs
startup.Configure(app, app.Environment);

app.Run();
