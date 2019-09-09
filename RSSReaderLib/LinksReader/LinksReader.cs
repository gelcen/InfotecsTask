using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RSSReaderLib.LinksReader
{
    public class LinksReader : ILinksReader
    {
        private readonly string _fileName = "links.json";

        private static readonly JsonSerializerSettings _settings = 
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

        public IEnumerable<string> GetUrls()
        {
            if (!File.Exists(_fileName))
            {
                throw new 
                    ArgumentException("Error: There is no file with RSS feed links(\"links.json\").");
            }

            var file = new StreamReader(_fileName);

                var items = file.ReadToEnd();

            var links = JsonConvert.DeserializeObject<List<string>>(items, _settings);

            file.Close();

            return links;
        }
    }
}
