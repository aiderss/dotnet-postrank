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
using System.Drawing;
using System.Xml;

namespace PostRank
{
    public class Post : SimpleElement
    {
        public static Post Create(string xmlData)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            return new Post(xmlDocument.DocumentElement);
        }

        public static Post Create(XmlElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return new Post(element);
        }

        private Post(XmlElement element)
            : base(element)
        {
        }

        public string Title
        {
            get { return this["title"]; }
        }

        public Uri Link
        {
            get { return GetUri("link"); }
            internal set { SetUri("link", value); }
        }

        public string Guid
        {
            get { return this["guid"]; }
        }

        public Uri Uri
        {
            get { return GetUri("aiderss:uri"); }
        }

        public string Source
        {
            get { return this["source"]; }
        }

        public Uri SourceUrl
        {
            get { return element["source"] == null ? null : new Uri(element["source"].GetAttribute("url")); }
        }

        public long CommentCount
        {
            get { return GetLong("slash:comments"); }
        }

        public DateTime PubDate
        {
            get { return GetDateTime("pubDate"); }
        }

        public string Description
        {
            get { return this["description"]; }
        }

        public string Content
        {
            get { return this["content:encoded"]; }
        }

        public Uri OriginalLink
        {
            get { return GetUri("aiderss:original_link"); }
            internal set { SetUri("aiderss:original_link", value); }
        }

        public double PostRank
        {
            get { return GetDouble("aiderss:postrank"); }
        }

        public Color PostRankColor
        {
            get { return GetColor("aiderss:postrank_color"); }
        }
    }
}