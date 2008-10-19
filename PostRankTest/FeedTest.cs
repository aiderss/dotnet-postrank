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
    public abstract class FeedTest : RequestTest
    {
        protected Feed feed;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            feed = new FeedFactory(TestUtil.AppKey, processor).GetFeed(TestUtil.FeedId);
        }

        [Test]
        public void Should_use_feed_id_as_request_parameter()
        {
            SendRequest();
            Assert.AreEqual(TestUtil.FeedId.ToString(), processor.GetParameterValue("feed_id"));
        }
    }
}