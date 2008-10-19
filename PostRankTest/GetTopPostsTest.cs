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
    public class GetTopPostsTest : FeedTest
    {
        public override void SendRequest()
        {
            feed.GetTopPosts(9, 1);
        }

        public override string RequestMethod
        {
            get { return "top_posts"; }
        }

        [Test]
        public void Should_use_requested_period_enum_as_request_parameter()
        {
            feed.GetTopPosts(Period.Day, 1);
            Assert.AreEqual("Day", processor.GetParameterValue("period"));
        }

        [Test]
        public void Should_use_requested_period_integer_as_request_parameter()
        {
            feed.GetTopPosts(1000, 1);
            Assert.AreEqual("1000", processor.GetParameterValue("period"));
        }

        [Test]
        public void Should_use_requested_count_as_request_parameter()
        {
            feed.GetTopPosts(9, 1);
            Assert.AreEqual("1", processor.GetParameterValue("num"));

            feed.GetTopPosts(9, 10);
            Assert.AreEqual("10", processor.GetParameterValue("num"));
        }

        [Test]
        public void Should_return_a_post_for_each_rss_item()
        {
            processor.Response = "<rss><channel></channel></rss>";
            Assert.AreEqual(0, TestUtil.GetPostCount(feed.GetTopPosts(9, 10)));

            processor.Response = "<rss><channel><item><link/></item></channel></rss>";
            Assert.AreEqual(1, TestUtil.GetPostCount(feed.GetTopPosts(9, 10)));

            processor.Response = "<rss><channel><item><link/></item><item><link/></item></channel></rss>";
            Assert.AreEqual(2, TestUtil.GetPostCount(feed.GetTopPosts(9, 2)));
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
