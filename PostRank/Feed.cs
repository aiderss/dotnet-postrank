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
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PostRank
{
    public class Feed
    {
        public static readonly string Host = "api.postrank.com";
        public static readonly string Version = "1";

        private readonly string appKey;
        private readonly long id;
        private readonly RequestProcessor processor;
        private SimpleElement feedData;

        internal Feed(string appKey, long id, RequestProcessor processor)
        {
            if (string.IsNullOrEmpty(appKey))
                throw new ArgumentException("appKey");

            if (processor == null)
                throw new ArgumentException("processor");

            this.appKey = appKey;
            this.id = id;
            this.processor = processor;
        }

        public long Id
        {
            get { return id; }
        }

        public string Title
        {
            get { return GetFeedData()["title"]; }
        }

        public string Description
        {
            get { return GetFeedData()["description"]; }
        }

        public DateTime PubDate
        {   
            get { return GetFeedData().GetDateTime("pubDate"); }
        }

        public long Ttl
        {
            get { return GetFeedData().GetLong("ttl"); }
        }

        public long StartIndex
        {
            get { return GetFeedData().GetLong("opensearch:startIndex"); }
        }

        public long ItemsPerPage
        {
            get { return GetFeedData().GetLong("opensearch:itemsPerPage"); }
        }

        public Uri Link
        {
            get { return GetFeedData().GetUri("link"); }
        }

        public DateTime LastBuildDate
        {
            get { return GetFeedData().GetDateTime("lastBuildDate"); }
        }

        public string ImageTitle
        {
            get { return GetFeedData()["image", "title"]; }
        }

        public Uri ImageUrl
        {
            get { return GetFeedData().GetUri("image", "url"); }
        }

        public Uri ImageLink
        {
            get { return GetFeedData().GetUri("image", "link"); }
        }

        public string this[string name]
        {
            get { return GetFeedData()[name]; }
        }

        public byte[] GetSparkline(Level level)
        {
            return processor.GetResponseAsBytes(GetSparklineUrl(level), null);
        }

        public string GetSparklineUrl(Level level)
        {
            return BuildRequest("sparkline", "png", "level="+level.ToString().ToLower());
        }

        public IEnumerable<Post> GetPostRank(IEnumerable<Uri> uris, IEnumerable<Feed> feeds)
        {
            StringBuilder postContents = new StringBuilder();

            foreach (Uri uri in uris)
            {
                if (uri != null)
                {
                    postContents.Append(String.Format("{0}url[]={1}", postContents.Length > 0 ? "&" : "", uri));
                }
            }

            if (postContents.Length == 0)
                throw new ArgumentException("Must provide at least one non-null uri", "uris");

            if (feeds != null)
            {
                foreach (Feed feed in feeds)
                {
                    postContents.Append(String.Format("{0}feed_id[]={1}", postContents.Length > 0 ? "&" : "", feed.Id));
                }
            }

            return GetPostsFromResponseTo(BuildRequest("postrank", "rss"), postContents.ToString());
        }

        public IEnumerable<Post> GetPosts(double level, int num, int start)
        {
            return GetPosts((object)level, num, start);
        }

        public IEnumerable<Post> GetPosts(Level level, int num, int start)
        {
            return GetPosts(level.ToString().ToLower(), num, start);
        }

        private IEnumerable<Post> GetPosts(object level, int num, int start)
        {
            return GetPostsFromResponseTo(BuildRequest("feed", "rss", "level=" + level, "num=" + num, "start=" + start), null);
        }

        public IEnumerable<Post> GetTopPosts(int period, int num)
        {
            return GetTopPosts((object)period, num);
        }

        public IEnumerable<Post> GetTopPosts(Period period, int num)
        {
            return GetTopPosts((object)period, num);
        }

        private IEnumerable<Post> GetTopPosts(Object period, int num)
        {
            return GetPostsFromResponseTo(BuildRequest("top_posts", "rss", "period="+period, "num="+num), null);
        }

        private IList<Post> GetPostsFromResponseTo(string request, string postContents)
        {
            IList<Post> posts = new List<Post>();

            foreach (XmlElement item in GetResponse(request, postContents).SelectNodes("//item"))
            {
                Post post = Post.Create(item);
                posts.Add(post);
                if (post.OriginalLink == null)
                {
                    post.OriginalLink = post.Link;
                    post.Link = null;
                }
            }

            if (posts.Count > 0 && posts[0].Description != null && posts[0].Description.Contains("{\"error\"=>\""))
            {
                throw new PostRankException(posts[0].Description);
            }

            return posts;
        }

        private string BuildRequest(string requestName, string responseFormat, params String[] parameters)
        {
            StringBuilder request = new StringBuilder(string.Format("http://{0}/v{1}/{2}?format={3}&appkey={4}&feed_id={5}", Host, Version, requestName, responseFormat, appKey, id));

            foreach (string parameter in parameters)
            {
                request.Append('&').Append(parameter);
            }

            return request.ToString();
        }

        private SimpleElement GetFeedData()
        {
            if (feedData == null)
            {
                feedData = new SimpleElement(GetResponse(BuildRequest("feed", "rss"), null)["channel"]);
            }

            return feedData;
        }

        private XmlElement GetResponse(string request, string postContents)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(processor.GetResponse(request, postContents));
            return document.DocumentElement;
        }
    }
}