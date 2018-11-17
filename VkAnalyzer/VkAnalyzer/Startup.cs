﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using VkAnalyzer.BL;
using VkAnalyzer.DbModels;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer
{
    public class Startup
    {
        private readonly string vkUsersInfoLogin;
        private readonly string vkUsersInfoPassword;
        private readonly string fileDataPath;

        public Startup(IConfiguration configuration)
        {
            var lines = File.ReadAllLines("settings.txt");
            vkUsersInfoLogin = lines[1];
            vkUsersInfoPassword = lines[2];
            fileDataPath = lines[3];

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<UsersDbContext>(o => o.UseSqlServer(string.Format(dbConnectionString, nameof(UsersDbContext))));

            services.AddSingleton<IUserInfoSource, VkUserInfoSource>(s => new VkUserInfoSource(vkUsersInfoLogin, vkUsersInfoPassword));
            services.AddSingleton<IDataSaver, FileDataSaver>(s => new FileDataSaver(fileDataPath));
            services.AddSingleton<IDataReader, FileDataReader>(s => new FileDataReader(fileDataPath));
            services.AddSingleton<IInterrogator, Interrogator>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IInterrogator interrogator)
        {
            Task.Factory.StartNew(async () =>
            {
                var dbContext = new UsersDbContext();
                var users = dbContext.Users.Select(u => u.UserId).ToList();

                interrogator.AddUsers(users);
                await interrogator.Start();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
