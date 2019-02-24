using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;
using VkAnalyzer.BE;
using VkAnalyzer.BL;
using VkAnalyzer.Extensions;
using VkAnalyzer.Interfaces;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<UsersDbContext>(o => o.UseSqlServer(string.Format(connection)));
            services.Configure<VkAnalyzerSettings>(Configuration.GetSection(nameof(VkAnalyzerSettings)));
            services.Configure<MongoConnectionSettings>(Configuration.GetSection(nameof(MongoConnectionSettings)));
            services.Configure<FileRepositorySettings>(Configuration.GetSection(nameof(FileRepositorySettings)));

            services.AddVkUserInfoSource();
            //services.AddDummyUserInfoSource();
            services.AddFileRepository();

            services.AddSingleton<ITracker, Tracker>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            //});

            // Register the Swagger services
            services.AddSwaggerDocument();

            //services.AddBundling()
            //    .UseDefaults(_environment)
            //    .UseNUglify();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ITracker tracker,
            IUsersRepository usersRepository)
        {
            Task.Factory.StartNew(async () =>
            {
                var users = await usersRepository.GetUsersAsync();

                tracker.AddUsers(users);
                await tracker.Start();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.WithOrigins("http://localhost:3000")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
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

            //app.UseBundling(bundles =>
            //{
            //    bundles.AddJs("/runtime-main.js")
            //        .Include("/js/runtime~main.*");

            //    bundles.AddJs("/vendor.js")
            //        .Include("/js/vendor.*");

            //    bundles.AddJs("/main.js")
            //        .Include("/js/main.*");
            //});

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
