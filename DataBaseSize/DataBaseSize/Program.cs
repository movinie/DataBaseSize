// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>This is the Program class.</summary>
namespace DataBaseSize
{
    using System;
    using DataBaseSize.Configurations;
    using DataBaseSize.IServices;
    using System.Threading;
    using System.Threading.Tasks;
    using DataBaseSize.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            BuildContainer(services);
            var serviceProvider = services.BuildServiceProvider();

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                cts.Cancel();
                e.Cancel = true;
            };

            try
            {
                await serviceProvider.GetService<Application>()
                    .Run(cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e);
                throw;
            }
        }      

        private static void BuildContainer(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            services.AddOptions();
            services.Configure<ApplicationConfig>(x => configuration.GetSection("Application").Bind(x));
            services.Configure<GoogleServiceConfig>(x => configuration.GetSection("GoogleService").Bind(x));
            services.AddSingleton<IGoogleSheetsService, GoogleSheetsService>();
            services.AddTransient<IDatabaseInfoService, DatabaseInfoService>();
            services.AddTransient<Application>();
        }
    }
}
