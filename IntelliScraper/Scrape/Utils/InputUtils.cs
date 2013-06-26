using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelliScraper.Scrape
{
    public class InputUtils
    {
        /// <summary>
        /// Get list of values by input type 
        /// </summary>
        public static List<string> getListFromInputByType(object input, string inputAttributeKey)
        {
            List<string> links = new List<string>();

            //single
            if (input.GetType() == typeof(string))
                links.Add((string)input);

            if (input.GetType() == typeof(List<string>))
                links.AddRange((List<string>)input);

            if (input.GetType() == typeof(Model.LoopLinkResult))
            {
                Model.LoopLinkResult m = (Model.LoopLinkResult)input;
                links.AddRange(getFiles(m.xpathResultSingle, inputAttributeKey));
                links.AddRange(getFiles(m.xpathResultCollection, inputAttributeKey));
            }

            if (input.GetType() == typeof(List<Model.LoopLinkResult>))
            {
                List<Model.LoopLinkResult> mm = (List<Model.LoopLinkResult>)input;
                foreach (Model.LoopLinkResult m in mm)
                {
                    links.AddRange(getFiles(m.xpathResultSingle, inputAttributeKey));
                    links.AddRange(getFiles(m.xpathResultCollection, inputAttributeKey));
                }
            }


            //Get all link from input attribute by input type

            if (input.GetType() == typeof(List<KeyValuePair<string, object>>))
            {
                List<KeyValuePair<string, object>> data = (List<KeyValuePair<string, object>>)input;

                foreach (KeyValuePair<string, object> d in data)
                {
                    if (d.Key == inputAttributeKey)
                    {
                        links.Add((string)d.Value);
                        break;
                    }
                }
            }
            if (input.GetType() == typeof(List<List<KeyValuePair<string, object>>>))
            {
                List<List<KeyValuePair<string, object>>> dtList = (List<List<KeyValuePair<string, object>>>)input;

                foreach (List<KeyValuePair<string, object>> data in dtList)
                {
                    foreach (KeyValuePair<string, object> d in data)
                    {
                        if (d.Key == inputAttributeKey)
                        {
                            links.Add((string)d.Value);
                            break;
                        }
                    }
                }
            }

            return links;
        }

        public static string getValueFromInputByType(object input, string inputAttributeKey)
        {
            List<string> vals = getListFromInputByType(input, inputAttributeKey);
            if (vals.Count > 0)
                return vals[0];
            return string.Empty; ;
        }


        /// <summary>
        /// Get files from (List<KeyValuePair<string, object>> 
        /// </summary>
        private static List<string> getFiles(List<KeyValuePair<string, object>> val, string inputAttributeKey)
        {
            List<string> files = new List<string>();
            if (val != null)
            {
                foreach (KeyValuePair<string, object> v in val)
                {

                    if (v.Key == inputAttributeKey)
                        files.Add((string)v.Value);

                }
            }
            return files;
        }

        /// <summary>
        /// Get files from List<List<KeyValuePair<string, object>>>
        /// </summary>
        private static List<string> getFiles(List<List<KeyValuePair<string, object>>> vals, string inputAttributeKey)
        {
            List<string> files = new List<string>();
            if (vals != null)
            {
                foreach (List<KeyValuePair<string, object>> val in vals)
                {
                    if (val != null)
                    {
                        foreach (KeyValuePair<string, object> v in val)
                        {
                            if (v.Key ==inputAttributeKey)
                                files.Add((string)v.Value);

                        }
                    }
                }
            }
            return files;
        }
    }
}
