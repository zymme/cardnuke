using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using MyCardNukeSub.Readers;
using MyCardNukeSub.Subscriptions;
using MyCardNukeSub.Options;
using MyCardNukeDataLib.Context;
using MyCardNukeDataLib.Repository;

namespace MyCardNukeSub
{
    class Program
    {
        static IConfiguration Configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello CardNuke !");
            //Console.WriteLine("Read From EventStore for addNewCard");
            Console.WriteLine("Sub to cardtxgroup to pickup new events for charge card events");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            try 
            {
                var serviceProvider = new ServiceCollection()
                    .AddLogging()
                    .AddOptions()
                    .AddDbContext<CardContext>(options => options.UseNpgsql(Configuration.GetSection("CardAccessPostgreSqlProvider").Value), ServiceLifetime.Transient)
                    .AddTransient<ICardRepository, CardRepository>()
                    .AddSingleton<PersistentSubcription>()

                    .BuildServiceProvider();

                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var serilog = new LoggerConfiguration()
                    .Enrich.FromLogContext();

                serilog.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}");

                loggerFactory.AddSerilog(serilog.CreateLogger());

                var consumer = serviceProvider.GetService<PersistentSubcription>();
                consumer.Start();

                //reader.ReadFromBeginning();
                //ReadFromEventStore reader = new ReadFromEventStore();

                //PersistentSubcription persistentSubscription = new PersistentSubcription();
                //persistentSubscription.ProcessChargeEvents();

                //persistentSubscription.Close();

            }
            catch(Exception e)
            {
                Console.WriteLine($"Problem occured in reader : {e.Message}");
            }

        }
    }
}
