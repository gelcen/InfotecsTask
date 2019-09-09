using RSSReaderLib.FeedManager;
using RSSReaderLib.LinksReader;
using RSSReaderLib.MessageWriter;
using System;
using System.Collections.Generic;
using System.Xml;

namespace RSSReader
{
    class Program
    {
        private static ILinksReader _linksReader;

        private static IMessageWriter _messageWriter;

        private static IFeedManager<XmlDocument> _feedManager;

        private static Dictionary<string, Action> _commands;

        static void Main(string[] args)
        {
            Initialize();
            Console.WriteLine("Welcome to RSS Loader program.");
            Help();
            Console.WriteLine("Please enter command:");
            while (true)
            {
                var line = Console.ReadLine();
                try
                {
                    if (line == "exit")
                    {
                        break;
                    }
                    else if (_commands.ContainsKey(line))
                    {
                        _commands[line]?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void Initialize()
        {
            _linksReader = new LinksReader();
            _messageWriter = new ConsoleMessageWriter();
            _feedManager = new FeedManager(_linksReader, _messageWriter);

            _commands = new Dictionary<string, Action>();
            _commands.Add("remove", () => _feedManager.RemoveFeeds());
            _commands.Add("pull", () => _feedManager.SaveFeeds());
            _commands.Add("list", new Action(List));
            _commands.Add("help", new Action(Help));
        }

        private static void List()
        {
            var feeds = _feedManager.ReadFeeds();

            foreach (var feed in feeds)
            {
                XmlNodeList rssNodes = feed.SelectNodes("rss/channel/item");

                foreach (XmlNode item in rssNodes)
                {
                    Console.WriteLine(item.SelectSingleNode("title").InnerText);
                    Console.WriteLine(item.SelectSingleNode("pubDate").InnerText);
                    Console.WriteLine(item.SelectSingleNode("link").InnerText);
                    Console.WriteLine(Environment.NewLine);
                }
            }
        }

        private static void Help()
        {
            Console.WriteLine("help -- shows the list of commands and what they do");
            Console.WriteLine("pull -- takes urls from links.json file and saves feeds in current directory");
            Console.WriteLine("list -- reads feeds from current directory and shows item name, publication date and link");
            Console.WriteLine("remove -- deletes all feeds in current directory");
            Console.WriteLine("exit -- closes the program");
        }
    }
}
