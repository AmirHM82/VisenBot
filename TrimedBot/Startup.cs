using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using TrimedBot.Core.Classes;
using TrimedBot.Core.Interfaces;
using TrimedBot.Core.Services;
using TrimedBot.DAL.Context;
using TrimedBot.DAL.Entities;
using Microsoft.Extensions.Caching.Distributed;
using TrimedBot.DAL;

namespace TrimedBot
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
            services.AddDbContext<DB>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQL"));
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);

            services.AddServices();

            services.AddControllers();
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            //app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            //Migrate.Entities(app.ApplicationServices);

            app.ApplicationServices.GetRequiredService<BotServices>().StartReceiving();
            app.ApplicationServices.GetRequiredService<ResponseService>().StartProccesingMessages();
        }
    }

    public static class Services
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IMedia, MediaServices>();
            services.AddTransient<IUser, UserServices>();
            services.AddTransient<IToken, TokenServices>();
            services.AddTransient<UpdateServices>();
            services.AddTransient<ITempMessage, TempMessageServices>();
            services.AddTransient<ISettings, SettingsServices>();
            services.AddTransient<IBanner, BannerServices>();
            services.AddTransient<ITag, TagService>();
            services.AddScoped<ObjectBox>();
            services.AddSingleton<ResponseService>();
            services.AddSingleton<BotServices>();
        }
    }
}
