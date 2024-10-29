var builder = WebApplication.CreateBuilder(args);

// יצירת מופע של Startup
var startup = new MyMvcApp.Startup(builder.Services.BuildServiceProvider().GetRequiredService<ILogger<MyMvcApp.Startup>>());

// רישום השירותים מתוך Startup.cs
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// הגדרת ה-middleware מתוך Startup.cs
startup.Configure(app, app.Environment);

app.Run();
