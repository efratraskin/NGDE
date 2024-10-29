using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; // נדרש כדי להשתמש בלוגים
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace MyMvcApp
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        // Constructor שמקבל את ILogger<Startup> דרך תלות
        public Startup(ILogger<Startup> logger)
        {
            _logger = logger;
        }

        // שיטה זו משמשת לרישום שירותים לאפליקציה
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Starting to configure services.");

            services.AddControllers();
            _logger.LogInformation("Controllers service added.");

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            _logger.LogInformation("CORS policy 'AllowOrigin' added.");

            // רישום MVC עם Newtonsoft.Json
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            _logger.LogInformation("Controllers with Views and Newtonsoft.Json configured.");
        }

        // שיטה זו משמשת להגדרת האפליקציה
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _logger.LogInformation("Starting to configure middleware.");

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            _logger.LogInformation("CORS policy applied.");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // עמוד שגיאות לפיתוח
                _logger.LogInformation("Development environment: Developer exception page enabled.");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // טיפול בשגיאות
                app.UseHsts(); // HSTS להגברת האבטחה
                _logger.LogInformation("Production environment: HSTS and error handling configured.");
            }

            app.UseHttpsRedirection(); // הפניה ל-HTTPS
            _logger.LogInformation("HTTPS redirection enabled.");

            app.UseStaticFiles(); // שימוש בקבצים סטטיים
            _logger.LogInformation("Static files enabled.");

            app.UseRouting(); // הפעלת routing
            _logger.LogInformation("Routing middleware configured.");

            app.UseAuthorization(); // הפעלת אוטוריזציה
            _logger.LogInformation("Authorization middleware configured.");

            // הגדרת מסלולי הקצה
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // כאן מפה את ה-API Controllers
                _logger.LogInformation("Controllers mapped.");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                _logger.LogInformation("Default route mapped to Home/Index.");
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"Photos")),
                RequestPath="/Photos"
            });

            _logger.LogInformation("Middleware configuration completed.");
        }
    }
}
