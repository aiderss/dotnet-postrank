/******************************************************************************
 * PostRank 1.0
 * Copyright 2008 Declan Whelan
 * Visit http://dpwhelan.com/postrank
 * 
 * This code is relased under the Creative Commons Attribution 3.0 License
 * http://creativecommons.org/licenses/by/3.0/
 * 
 * You are free to reuse and modify this code as long as your attribte 
 * the original author: Declan Whelan http://dpwhelan.com
 *******************************************************************************/
using System;
using System.Xml;

namespace PostRank
{
    public class FeedFactory
    {
        private readonly string appKey;
        private readonly RequestProcessor processor;

        public FeedFactory(string appKey)
            : this(appKey, new WebRequestProcessor())
        {
        }

        public FeedFactory(string appKey, RequestProcessor processor)
        {
            if (string.IsNullOrEmpty(appKey))
                throw new ArgumentException("appKey");

            if (processor == null)
                throw new ArgumentException("processor");

            this.appKey = appKey;
            this.processor = processor;
        }

        public Feed GetFeed(string url)
        {
            return GetFeed(url, 100);
        }

        public Feed GetFeed(string url, int priority)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("url");

            string request = string.Format("http://{0}/v{1}/feed_id?appkey={2}&format=xml&link={3}&priority={4}",
                                           Feed.Host, Feed.Version, appKey, url, priority);

            string response = processor.GetResponse(request, null);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);

            XmlNode feedId = doc.SelectSingleNode("//feed_id");

            if (feedId == null)
            {
                string message = "Could not get feed";

                if (doc.SelectSingleNode("//error") != null)
                {
                    message += ": " + doc.SelectSingleNode("//error").InnerText;
                }
                throw new PostRankException(message);
            }
            else
            {
                return GetFeed(long.Parse(feedId.InnerText));
            }
        }

        public Feed GetFeed(long id)
        {
            return new Feed(appKey, id, processor);
        }
    }
}