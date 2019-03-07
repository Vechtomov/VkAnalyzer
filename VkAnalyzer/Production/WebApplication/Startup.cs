using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;
using VkAnalyzer.BE;
using VkAnalyzer.BL;
using VkAnalyzer.BL.SQL;
using VkAnalyzer.Interfaces;
using WebApplication.Extensions;
using WebApplication.Settings;

namespace WebApplication
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
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

			services.AddUserInfoSource();
			services.AddRepository();

			services.AddSingleton<ITracker, Tracker>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger services
            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ITracker tracker,
            IUsersRepository usersRepository,
	        DbContextOptions<UsersDbContext> options)
        {
            Task.Factory.StartNew(async () =>
            {
                var userIds = (await usersRepository.GetUsersAsync()).Select(u => u.Id);

                tracker.AddUsers(userIds);
                await tracker.Start();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:3000")
                    .AllowCredentials()
                );
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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
