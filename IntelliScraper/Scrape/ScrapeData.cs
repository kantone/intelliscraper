using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape
{
    public class ScrapeData
    {
        public string Id { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }
        public string MappingName { get; set; }

        public ScrapeData()
        {

        }
        public ScrapeData(string Id, object Value, string Type, string MappingName)
        {
            this.Id = Id;
            this.Value = Value;
            this.Type = Type;
            this.MappingName = MappingName;
        }
    }
}
