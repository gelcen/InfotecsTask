using System.Collections.Generic;

namespace RSSReaderLib.LinksReader
{
    public interface ILinksReader
    {
        IEnumerable<string> GetUrls();
    }
}
