using RSSReaderLib.LinksReader;
using RSSReaderLib.MessageWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace RSSReaderLib.FeedManager
{
    public class FeedManager : IFeedManager<XmlDocument>
    {
        private readonly ILinksReader _linksReader;
        private readonly IMessageWriter _messageWriter;

        public FeedManager(ILinksReader linksReader,
                           IMessageWriter messageWriter)
        {
            _linksReader = linksReader;
            _messageWriter = messageWriter;
        }

        public IEnumerable<XmlDocument> ReadFeeds()
        {
            var feedList = new List<XmlDocument>();

            foreach (var feed in GetXmls())
            {
                _messageWriter.Write($"Reading {feed.Name}...");

                XmlDocument doc = new XmlDocument();

                doc.Load(feed.Name);

                feedList.Add(doc);
            }

            _messageWriter.Write("Reading feeds has been done");

            return feedList;
        }

        public void RemoveFeeds()
        {
            foreach (var feed in GetXmls())
            {
                File.Delete(feed.FullName);
            }

            _messageWriter.Write("Removing feeds has been done");
        }

        public void SaveFeeds()
        {
            var tasks = new List<Task>();
            foreach (var url in _linksReader.GetUrls())
            {
                tasks.Add(Task.Factory.StartNew(
                    () => SaveOneFeed(url)
                    ));
            }
            Task.WaitAll(tasks.ToArray());
            _messageWriter.Write("Saving feeds has been done");
        }

        private void SaveOneFeed(string url)
        {
            _messageWriter.Write($"Saving {url} ...");

            XmlDocument doc = new XmlDocument();

            var reader = XmlReader.Create(url);

            doc.Load(reader);

            Uri uri = new Uri(url);
            var name = uri.Host;

            var timeStamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            timeStamp = timeStamp.Replace(":", "_");
            timeStamp = timeStamp.Replace(" ", "_");

            doc.Save(name + timeStamp + ".xml");

            reader.Close();
        }

        private FileInfo[] GetXmls()
        {
            DirectoryInfo currentFolder =
                new DirectoryInfo(Directory.GetCurrentDirectory());

            FileInfo[] feeds = currentFolder.GetFiles("*.xml");

            return feeds;
        }
    }
}
