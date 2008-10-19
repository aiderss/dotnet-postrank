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
using NUnit.Framework;
using PostRank;

namespace PostRankTest
{
    [TestFixture]
    public class GetPostsTest : FeedTest
    {
        public override void SendRequest()
        {
            feed.GetPosts(Level.All, 1, 0);
        }

        public override string RequestMethod
        {
            get { return "feed"; }
        }

        [Test]
        public void Should_use_level_as_level_parameter()
        {
            feed.GetPosts(Level.All, 1, 9);
            Assert.AreEqual("all", processor.GetParameterValue("level"));

            feed.GetPosts(9.5, 10, 9);
            Assert.AreEqual("9.5", processor.GetParameterValue("level"));
        }

        [Test]
        public void Should_use_requested_count_as_request_parameter()
        {
            feed.GetPosts(Level.All, 1, 9);
            Assert.AreEqual("1", processor.GetParameterValue("num"));

            feed.GetPosts(Level.All, 10, 9);
            Assert.AreEqual("10", processor.GetParameterValue("num"));
        }

        [Test]
        public void Should_use_requested_start_as_request_parameter()
        {
            feed.GetPosts(Level.All, 9, 1);
            Assert.AreEqual("1", processor.GetParameterValue("start"));

            feed.GetPosts(Level.All, 9, 10);
            Assert.AreEqual("10", processor.GetParameterValue("start"));
        }

        [Test]
        public void Should_return_a_post_for_each_rss_item()
        {
            processor.Response = "<rss><channel><item><link/></item></channel></rss>";
            Assert.AreEqual(1, TestUtil.GetPostCount(feed.GetPosts(Level.All, 10, 1)));

            processor.Response = "<rss><channel><item><link/></item><item><link/></item></channel></rss>";
            Assert.AreEqual(2, TestUtil.GetPostCount(feed.GetPosts(Level.All, 10, 1)));
        }

        [Test]
        [ExpectedException(typeof(PostRankException))]
        public void Should_throw_exception_if_response_indicates_an_error()
        {
            processor.Response =
                "<rss><channel><item><title>errorNum is missing or invalid</title><description>{\"error\"=>\"Num is missing or invalid\"}</description></item></channel></rss>";
            SendRequest();
        }
    }
}