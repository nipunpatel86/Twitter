using System;
using System.Threading;

namespace Twitter
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitterStream twitterStream = new TwitterStream();
            var success = twitterStream.StartStreamAsync();

            if (Console.ReadKey().Key == ConsoleKey.C)
            {
                twitterStream.StopStream();
            }

        }
    }
}
