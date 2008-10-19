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
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PostRank;

namespace PostRankTest
{
    [TestFixture]
    public class GetPostRankTest : FeedTest
    {
        public override void SendRequest()
        {
            Uri[] urls = new Uri[] { new Uri("http://foo.com/") };
            feed.GetPostRank(urls, null);
        }

        public override string RequestFormat
        {
            get { return "rss"; }
        }

        public override string RequestMethod
        {
            get { return "postrank"; }
        }

        [Test]
        public void Should_pass_a_single_url_as_a_post_parameter()
        {
            feed.GetPostRank(new Uri[] { new Uri("http://foo.com/")}, null);
            Assert.AreEqual("url[]=http://foo.com/", processor.PostContents);
        }

        [Test]
        public void Should_pass_multiple_urls_with_ampersand_separator_as_post_parameters()
        {
            feed.GetPostRank(new Uri[] { new Uri("http://foo.com/"), new Uri("http://bar.com/") }, null);
            Assert.AreEqual("url[]=http://foo.com/&url[]=http://bar.com/", processor.PostContents);
        }

        [Test]
        public void Should_pass_a_single_feed_id_as_a_post_parameter()
        {
            feed.GetPostRank(new Uri[] { new Uri("http://foo.com/") }, new Feed[] { feed });
            Assert.AreEqual("url[]=http://foo.com/&feed_id[]=1", processor.PostContents);
        }

        [Test]
        public void Should_pass_multiple_feed_ids_with_ampersand_separator_as_post_parameters()
        {
            feed.GetPostRank(new Uri[] { new Uri("http://foo.com/"), new Uri("http://bar.com/") }, new Feed[] { feed, feed });
            Assert.AreEqual("url[]=http://foo.com/&url[]=http://bar.com/&feed_id[]=1&feed_id[]=1", processor.PostContents);
        }

        [Test]
        public void Should_ignore_null_uris()
        {
            feed.GetPostRank(new Uri[] { new Uri("http://foo.com/"), null }, null);
            Assert.AreEqual("url[]=http://foo.com/", processor.PostContents);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_exception_if_no_valid_uris_provided()
        {
            feed.GetPostRank(new Uri[] { null, null }, null);
        }

        [Test]
        public void Should_put_response_link_element_into_original_link_property()
        {
            processor.Response = "<rss><channel><item><link>http://foo.com/</link></item></channel></rss>";
            Post post = TestUtil.GetFirstPost(feed.GetPostRank(new Uri[] { new Uri("http://foo.com/") }, null));

            Assert.AreEqual("http://foo.com/", post.OriginalLink.ToString());
        }

        [Test]
        public void Should_return_a_post_for_each_rss_item()
        {
            processor.Response = "<rss><channel></channel></rss>";
            Assert.AreEqual(0, TestUtil.GetPostCount(feed.GetPostRank(new Uri[] { new Uri("http://foo.com/") }, null)));

            processor.Response = "<rss><channel><item><link/></item></channel></rss>";
            Assert.AreEqual(1, TestUtil.GetPostCount(feed.GetPostRank(new Uri[] { new Uri("http://foo.com/") }, null)));

            processor.Response = "<rss><channel><item><link/></item><item><link/></item></channel></rss>";
            Assert.AreEqual(2, TestUtil.GetPostCount(feed.GetPostRank(new Uri[] { new Uri("http://foo.com/") }, null)));
        }
    }
}