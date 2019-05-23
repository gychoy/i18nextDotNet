﻿using System;
using I18Next.Net;
using I18Next.Net.Backends;
using I18Next.Net.Extensions;
using I18Next.Net.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Console = System.Console; 

namespace Example.ConsoleApp.NetCore
{
    class Program
    {
        private static ITranslationBackend _backend;
        
        static void Main(string[] args)
        {
            SetupBackend();
            
            SampleOne();

            SampleTwo();

            Console.ReadKey();
        }

        private static void SetupBackend()
        {
            var backend = new InMemoryBackend();
            
            backend.AddTranslation("en", "translation", "exampleKey", "My English text.");
            backend.AddTranslation("de", "translation", "exampleKey", "Mein deutscher text.");

            _backend = backend;
        }

        private static void SampleOne()
        {
            Console.WriteLine("Sample one: Without Microsoft.Extensions.DependencyInjection");

            var translator = new DefaultTranslator(_backend);
            
            var i18next = new I18NextNet(_backend, translator);

            Console.WriteLine("English translation:");
            i18next.Language = "en";
            Console.WriteLine(i18next.T("exampleKey"));
            
            Console.WriteLine("German translation:");
            i18next.Language = "de";
            Console.WriteLine(i18next.T("exampleKey"));
            
            Console.WriteLine();
        }

        private static void SampleTwo()
        {
            Console.WriteLine("Sample one: Using Microsoft.Extensions.DependencyInjection");

            var services = new ServiceCollection();

            // Register I18Next.Net
            services.AddI18NextLocalization(builder => builder.AddBackend(_backend));
            
            using(var serviceProvider = services.BuildServiceProvider())
            using (var scope = serviceProvider.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;

                Console.WriteLine("The first example uses the II18Next interface for direct access to I18Next");
                
                var i18next = scopeProvider.GetService<II18Next>();

                Console.WriteLine("English translation:");
                i18next.Language = "en";
                Console.WriteLine(i18next.T("exampleKey"));
            
                Console.WriteLine("German translation:");
                i18next.Language = "de";
                Console.WriteLine(i18next.T("exampleKey"));
                
                
                Console.WriteLine();
                Console.WriteLine("The second example uses Microsofts IStringLocalizer interface for translations.");
                
                var localizer = scopeProvider.GetService<IStringLocalizer>();

                Console.WriteLine("English translation:");
                i18next.Language = "en";
                Console.WriteLine(localizer["exampleKey"]);
            
                Console.WriteLine("German translation:");
                i18next.Language = "de";
                Console.WriteLine(localizer["exampleKey"]);
                
                
                Console.WriteLine();
                Console.WriteLine("It is also possible to use Microsofts IStringLocalizer<T> interface for translations.");
                
                var localizerGeneric = scopeProvider.GetService<IStringLocalizer<Program>>();

                Console.WriteLine("English translation:");
                i18next.Language = "en";
                Console.WriteLine(localizerGeneric["exampleKey"]);
            
                Console.WriteLine("German translation:");
                i18next.Language = "de";
                Console.WriteLine(localizerGeneric["exampleKey"]);
                
                Console.WriteLine();
            }
        }
    }
}
