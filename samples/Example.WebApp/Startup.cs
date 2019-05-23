using I18Next.Net.AspNetCore;
using I18Next.Net.Backends;
using I18Next.Net.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Example.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            // Enable request localization in order to determine the users desired language based on the Accept-Language header.
            //app.UseRequestLocalization(options => options.AddSupportedCultures("de", "en"));

            //var staticFileExtensionsProvider = new FileExtensionContentTypeProvider();
            //staticFileExtensionsProvider.Mappings.Add(".json", "application/json");

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    if(!ctx.Context.Response.Headers.ContainsKey("Access-Control-Allow-Origin")) ctx.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    if (!ctx.Context.Response.Headers.ContainsKey("Access-Control-Allow-Headers")) ctx.Context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                },
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/locales")),
                RequestPath = "/locales"
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Option 1: Simple setup for AspNetCore using the default configuration
            services.AddI18NextLocalization(i18n => i18n
                .IntegrateToAspNetCore()
                .Configure(x => x.DetectLanguageOnEachTranslation = false)
                .AddBackend(new JsonFileBackend("wwwroot/locales"))
                .UseDefaultLanguage("en")
                .UseDefaultNamespace("translation")

            );

            // Option 2: Customize the locales location in order to use the same json files on the client side. 
            //services.AddI18NextLocalization(i18n =>
            //{
            //    i18n.IntegrateToAspNetCore()
            //        .AddBackend(new JsonFileBackend("wwwroot/locales"))
            //        .UseDefaultLanguage("en");
            //});

            services.AddCors();
            services.AddMvc()
                // Enable view localization and register required I18Next services
                .AddI18NextViewLocalization();
        }
    }
}
