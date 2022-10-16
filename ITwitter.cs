using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Twitter
{
    interface ITwitter
    {
        public Task<bool> StartStreamAsync();
        public void StopStream();
    }
}
