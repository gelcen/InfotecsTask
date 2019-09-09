using System.Collections.Generic;

namespace RSSReaderLib.FeedManager
{
    public interface IFeedManager<T>
    {
        void SaveFeeds();

        IEnumerable<T> ReadFeeds();

        void RemoveFeeds();
    }
}
