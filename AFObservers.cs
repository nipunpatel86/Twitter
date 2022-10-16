using System;
using System.Collections.Generic;
using System.Text;

namespace Twitter
{
    public class AFObservers
    {
        private AFElements _elements;

        public AFObservers(string server, string database)
        {
            PISystem ps = new PISystems()[server];
            AFDatabase db = ps.Databases[database];
            this._elements = db.Elements;
        }

        public void AddSubscribers(TwitterStream twitterStream)
        {
            foreach (AFElement element in this._elements)
            {
                TwitterPipe pipe = new TwitterPipe(element);
                pipe.SubscribeToStream(twitterStream);
            }
        }
    }
}
