using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events.V2;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Streaming.V2;

namespace Twitter
{
    public class TwitterStream:ITwitter
    {
        private ISampleStreamV2 _internalStream;
        private uint tweetcount=0;
        private uint lastcount;
        private volatile bool processsstart = true;
        public TwitterStream()
        {
            string accessToken = ConfigurationManager.AppSettings["accessToken"];
            string accessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"];
            string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
            string Bearertoken = ConfigurationManager.AppSettings["Bearer"];
            var TwitterCredentials = new TwitterCredentials(consumerKey, consumerSecret, Bearertoken);
            var userClient = new TwitterClient(TwitterCredentials);
            this._internalStream = userClient.StreamsV2.CreateSampleStream();
            //Signup the delegate for the TweetReceived event
            this._internalStream.TweetReceived += (sender, args) =>
            {
                ProcessTweet(sender, args);
            };

            Thread thread = new Thread(ProcessCount);
            thread.Start();

        }

        void ProcessCount()
        {
            Process Proc = Process.GetCurrentProcess();
            long AffinityMask = (long)Proc.ProcessorAffinity;

            ProcessThread Thread = Proc.Threads[0];
            AffinityMask = 0x0002; // use only the second processor, despite availability
            Thread.ProcessorAffinity = (IntPtr)AffinityMask;
            while (processsstart)
            {
                var tweetsec = tweetcount - lastcount;
                Console.WriteLine("TotalTweet:" + tweetcount + " Tweet/Sec:" + tweetsec + " ");
                lastcount = tweetcount;
                System.Threading.Thread.Sleep(1000);
            }
            Console.WriteLine("Process Thread Finished.");
        }
        private void ProcessTweet(object sender, TweetV2ReceivedEventArgs args)
        {
            tweetcount++;//Shared variable
        }
        public async Task<bool> StartStreamAsync()
        {
            Console.WriteLine("Start recieving from twitter");

            string error;
            try
            {
                await this._internalStream.StartAsync();
            }
            catch (TwitterException e)
            {
                error = e.ToString();
                return false;
            }
            catch (Exception ev)
            {
                error = ev.ToString();
                return false;
            }
            return true;
        }
        public void StopStream()
        {

            _internalStream.TweetReceived -= ProcessTweet;        
            _internalStream.StopStream();
            processsstart = false;
        }
    } //end class
}
