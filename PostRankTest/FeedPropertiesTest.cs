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
using NUnit.Framework;

namespace PostRankTest
{
    [TestFixture]
    public class FeedPropertiesTest : FeedTest
    {
        public override string RequestMethod
        {
            get { return "feed"; }
        }

        public override void SendRequest()
        {
            AccessPropertyToForceFirstRequestToBeIssued();
        }

        private void AccessPropertyToForceFirstRequestToBeIssued()
        {
            string title = feed.Title;
        }

        [Test]
        public void Title_property_should_return_title_element()
        {
            processor.Response = "<rss><channel><title>foo</title></channel></rss>";
            Assert.AreEqual("foo", feed.Title);
        }

        [Test]
        public void Description_property_should_return_description_element()
        {
            processor.Response = "<rss><channel><description>foo</description></channel></rss>";
            Assert.AreEqual("foo", feed.Description);
        }

        [Test]
        public void PubDate_property_should_return_pubDate_element()
        {
            processor.Response = "<rss><channel><pubDate>2/16/1992 12:15:12</pubDate></channel></rss>";
            Assert.AreEqual(DateTime.Parse("2/16/1992 12:15:12"), feed.PubDate);
        }

        [Test]
        public void Ttl_property_should_return_ttl_element()
        {
            processor.Response = "<rss><channel><ttl>99</ttl></channel></rss>";
            Assert.AreEqual(99, feed.Ttl);
        }

        [Test]
        public void Start_property_should_return_start_element()
        {
            processor.Response = @"<rss><channel><opensearch:startIndex xmlns:opensearch=""http://a9.com/-/spec/opensearch/1.1/"">99</opensearch:startIndex></channel></rss>";
            Assert.AreEqual(99, feed.StartIndex);
        }

        [Test]
        public void ItemsPerPage_property_should_return_items_per_page_element()
        {
            processor.Response = @"<rss><channel><opensearch:itemsPerPage xmlns:opensearch=""http://a9.com/-/spec/opensearch/1.1/"">99</opensearch:itemsPerPage></channel></rss>";
            Assert.AreEqual(99, feed.ItemsPerPage);
        }

        [Test]
        public void Link_property_should_return_link_element()
        {
            processor.Response = "<rss><channel><link>http://foo</link></channel></rss>";
            Assert.IsInstanceOfType(typeof(Uri), feed.Link);
            Assert.AreEqual(new Uri("http://foo"), feed.Link);
        }

        [Test]
        public void LastBuildDate_property_should_return_last_build_date_element()
        {
            processor.Response = "<rss><channel><lastBuildDate>2/16/1992 12:15:12</lastBuildDate></channel></rss>";
            Assert.AreEqual(DateTime.Parse("2/16/1992 12:15:12"), feed.LastBuildDate);
        }

        [Test]
        public void ImageTitle_property_should_return_image_title_element()
        {
            processor.Response = "<rss><channel><image><title>foo</title></image></channel></rss>";
            Assert.AreEqual("foo", feed.ImageTitle);
        }

        [Test]
        public void ImageUrl_property_should_return_image_url_element()
        {
            processor.Response = "<rss><channel><image><url>http://foo</url></image></channel></rss>";
            Assert.IsInstanceOfType(typeof(Uri), feed.ImageUrl);
            Assert.AreEqual(new Uri("http://foo"), feed.ImageUrl);
        }

        [Test]
        public void ImageUrl_property_should_be_null_if_image_element_is_missing()
        {
            processor.Response = "<rss><channel></channel></rss>";
            Assert.IsNull(feed.ImageUrl);
        }

        [Test]
        public void ImageLink_property_should_return_image_link_element()
        {
            processor.Response = "<rss><channel><image><link>http://foo</link></image></channel></rss>";
            Assert.IsInstanceOfType(typeof(Uri), feed.ImageLink);
            Assert.AreEqual(new Uri("http://foo"), feed.ImageLink);
        }

        [Test]
        public void ImageLink_property_should_be_null_if_image_element_is_missing()
        {
            processor.Response = "<rss><channel></channel></rss>";
            Assert.IsNull(feed.ImageLink);
        }

        [Test]
        public void This_indexer_should_return_named_element_text()
        {
            processor.Response = "<rss><channel><foo>bar</foo></channel></rss>";
            Assert.AreEqual("bar", feed["foo"]);
        }

        [Test]
        public void This_indexer_should_return_null_for_an_element_that_does_not_exist()
        {
            processor.Response = "<rss><channel></channel></rss>";
            Assert.IsNull(feed["bah"]);
        }
    }
}