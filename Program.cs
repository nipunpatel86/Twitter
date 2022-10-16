using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Microsoft.Extensions.Logging;

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

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");
            //do the actual work here
            var twitterStream = serviceProvider.GetService<ITwitter>();
            twitterStream.StartStreamAsync();

            logger.LogDebug("All done!");

            //TwitterStream twitterStream = new TwitterStream();
            //var success = twitterStream.StartStreamAsync();

            if (Console.ReadKey().Key == ConsoleKey.C)
            {
                twitterStream.StopStream();
            }

        }
    }
}
