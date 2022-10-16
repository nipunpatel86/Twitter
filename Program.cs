using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Twitter
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<ITwitter, TwitterStream>()
                .BuildServiceProvider();

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Starting application");
            
            var twitterStream = serviceProvider.GetService<ITwitter>();
            twitterStream.StartStreamAsync();

            if (Console.ReadKey().Key == ConsoleKey.C)
            {
                twitterStream.StopStream();
            }
            logger.LogInformation("All done!");
        }
    }
}
