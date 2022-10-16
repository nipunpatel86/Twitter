using Moq;
using System;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Streaming.V2;
using Xunit;

namespace Twitter.Test
{
    public class DataTest
    {
        int TweetCount = 0;
        ISampleStreamV2 _internalStream;

        [Theory]
        [InlineData("6ua2jlju7aDIBc5SMkIfNFjip",
                "KVV1s79bhKZwK1NPKsOjAWwdHF94Q1aeioeWisibXrNva779B4",
                "AAAAAAAAAAAAAAAAAAAAAFRLiAEAAAAAuGiGBGA%2BB5MlK3%2FBMaG90rLuBE0%3DVadxQwni0ge3K4pcycnlAIPnqAKT1eEzBS3K6GN0lnDCciGBwA")]
        public async void TweetRecieve_Test(string s1, string s2, string s3)
        {
            //Authentication
            //Arrange
            var userClient = new TwitterClient(s1,s2,s3
                );
            ////Act
            _internalStream = userClient.StreamsV2.CreateSampleStream();
            //Signup the delegate for the TweetReceived event
            _internalStream.TweetReceived += (sender, args) =>
            {
                UnitTest1_TweetReceived(sender, args);
            };
            try
            {
                await _internalStream.StartAsync();
            }
            catch (TwitterException e)
            {
              string  error = e.ToString();
            }
            catch (Exception ev)
            {
                string error = ev.ToString();
            }
            ////Assert
            Assert.Equal(1, TweetCount);
        }
        [Theory]
        [InlineData("test",
                "KVV1s79bhKZwK1NPKsOjAWwdHF94Q1aeioeWisibXrNva779B4",
                "Bearer")]
        public async void WrongCred_Test(string s1,string s2,string s3)
        {
            string error=string.Empty;
            //Authentication
            //Arrange
            var userClient = new TwitterClient(s1,s2,s3
                );
            ////Act
            _internalStream = userClient.StreamsV2.CreateSampleStream();
            //Signup the delegate for the TweetReceived event
            _internalStream.TweetReceived += (sender, args) =>
            {
                UnitTest1_TweetReceived(sender, args);
            };
            try
            {
                await _internalStream.StartAsync();
            }
            catch (TwitterException e)
            {
                error = e.ToString();
            }
            catch (Exception ev)
            {
                error = ev.ToString();
            }
            ////Assert
            Assert.Equal("", error);
        }
        private void UnitTest1_TweetReceived(object sender, Tweetinvi.Events.V2.TweetV2ReceivedEventArgs e)
        {
            TweetCount++;
            _internalStream.StopStream();
        }

    }
}
