using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.Extensions;
using NSwag.AspNetCore;
using VkAnalyzer.BE;
using VkAnalyzer.BL;
using VkAnalyzer.Extensions;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UsersDbContext>(o => o.UseSqlServer(string.Format(connection)));
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
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ITracker tracker, IUsersRepository usersRepository)
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUi();


            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            app.UseMvc();
        }
    }
}
