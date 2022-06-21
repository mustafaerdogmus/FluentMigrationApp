using FluentMigrationApp.Migrations;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;
using System.Reflection;

namespace FluentMigrationApp
{
    internal class Program
    {
        //Böylece.Net Console Application'da Dependency Injection'dan faydalanmış olacağız.İlk olarak framework servislerini Microsoft.Extensions.DependencyInjection altında olan ServiceCollection 'a ekliyoruz. Biz burada Fluent Migration kullanacağımız için  AddFluentMigratorCore() 'u ekliyoruz.Konfigurasyonu da bu aşamada yapabiliriz. Connection string'i MySQL mi SQL mi kullanacağımızı, Migration Sınıfmızı bu aşamada belirtiyoruz. Program.cs'nin Main metodunun son hali aşağıdaki gibi olmalı.

        private static void Main(string[] args)
        {
            Console.WriteLine("Migration POC Start!");

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment}.json", true, true)
                .AddEnvironmentVariables();

            var configurationRoot = builder.Build();
            var appConfig = configurationRoot.GetSection(nameof(AppConfig)).Get<AppConfig>();
            var connectionString = configurationRoot.GetConnectionString("DataContext");
            Console.WriteLine(appConfig.Environment.ToString());

            var services = new ServiceCollection()
            .AddFluentMigratorCore()
            // Configure the runner
            .ConfigureRunner(
            builder => builder
            // Use SQL
            .AddSqlServer2012()
            // The SQL connection string
            .WithGlobalConnectionString(connectionString)
            // Specify the assembly with the migrations
            //.WithMigrationsIn(typeof(InitialMigration).Assembly)
            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()

            ).Configure<RunnerOptions>(opt =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                switch (environment)
                {
                    case "Development":
                        //opt.Profile = "Development";
                        opt.Tags = new[] { "Send_To_Development", "Apply_Migration" };

                        break;

                    case "Test":
                        //opt.Profile = "Test";
                        opt.Tags = new[] { "Send_To_Test", "Apply_Migration" };
                        break;

                    case "Production":
                        //opt.Profile = "Production";
                        opt.Tags = new[] { "Send_To_Production", "Apply_Migration" };
                        break;

                    default:
                        break;
                }
            })
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);

            // add StructureMap
            var container = new Container();
            container.Configure(config =>
            {
                // Register stuff in container, using the StructureMap APIs...
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Program));
                    _.WithDefaultConventions();
                    _.AssemblyContainingType<IMigrationRunnerBuilder>();
                });
            });

            try
            {
                using (var scope = services.CreateScope())
                {
                    // Instantiate the runner
                    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                    runner.ListMigrations();
                    // Execute the migrations
                    runner.MigrateUp();
                    //runner.MigrateDown(202101261447);
                    //Execute the down scripts
                    //runner.RollbackToVersion(0);

                    Console.WriteLine("Migration has successfully executed.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Instantiate the runner
                using (var scope = services.CreateScope())
                {
                    // Instantiate the runner
                    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                    // Execute the migrations
                    runner.HasMigrationsToApplyRollback();
                    //runner.MigrateDown(202101261447);
                    //Execute the down scripts
                    //runner.RollbackToVersion(0);

                    Console.WriteLine("Migration rollback.");
                }
            }

            Console.ReadLine();
        }
    }
}