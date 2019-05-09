using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NSwag.AspNetCore;
using VkAnalyzer.BE;
using VkAnalyzer.BL;
using VkAnalyzer.Interfaces;
using VkAnalyzer.WebApp.Extensions;
using VkAnalyzer.WebApp.Middlewares;
using VkAnalyzer.WebApp.Settings;
using VkAnalyzer.WebApp.Validation;

namespace VkAnalyzer.WebApp
{
    public class Startup
    {
	    public Startup(IConfiguration configuration)
	    {
		    Configuration = configuration;
	    }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
			
			services.Configure<VkAnalyzerSettings>(Configuration.GetSection(nameof(VkAnalyzerSettings)));
            services.Configure<MongoConnectionSettings>(Configuration.GetSection(nameof(MongoConnectionSettings)));
            services.Configure<SqlRepositorySettings>(Configuration.GetSection(nameof(SqlRepositorySettings)));
			services.Configure<FileRepositorySettings>(Configuration.GetSection(nameof(FileRepositorySettings)));
			services.Configure<UserInfoSourceMode>(Configuration.GetSection(nameof(UserInfoSourceMode)));
			services.Configure<RepositoryMode>(Configuration.GetSection(nameof(RepositoryMode)));

	        services.AddLogger();

			services.AddUserInfoSource();
			services.AddRepository();

			services.AddSingleton<ITracker, Tracker>();

            services.AddMvc(options =>
            {
	            options.Filters.Add(typeof(ValidatorActionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger services
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ITracker tracker,
            IUsersRepository usersRepository,
	        IApplicationLifetime lifetime)
        {
	        lifetime.ApplicationStopping.Register(LogManager.Shutdown);

            Task.Factory.StartNew(async () =>
            {
                var userIds = (await usersRepository.GetUsersAsync()).Select(u => u.Id);

                tracker.AddUsers(userIds);
                await tracker.Start();
            });

            if (env.IsDevelopment())
            {
				app.UseCors(builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.WithOrigins("http://localhost:3000")
					.AllowCredentials()
				);
			}
            else
            {
                app.UseHsts();
            }

	        app.UseMiddleware<ExceptionMiddleware>();

			//app.UseHttpsRedirection();
			app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseMvc(routes =>
            {
                routes.MapRoute("api", "api/{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "Default",
                    template: "{*anything}",
                    defaults: new { controller = "Spa", action = "Index"}
                );
            });
        }
    }
}
