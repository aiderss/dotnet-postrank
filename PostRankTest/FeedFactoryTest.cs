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
using System.Net;
using NUnit.Framework;
using PostRank;

namespace PostRankTest
{
    [TestFixture]
    public class FeedFactoryTest : RequestTest
    {
        private static readonly string URL = "www.dpwhelan.com/blog";

        private FeedFactory feedFactory;

        public override void SendRequest()
        {
            feedFactory.GetFeed(URL);
        }

        public override string RequestMethod
        {
            get { return "feed_id"; }
        }

        public override string RequestFormat
        {
            get { return "xml"; }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            processor.Response = string.Format("<results><feed_id>0</feed_id></results>");
            feedFactory = new FeedFactory(TestUtil.AppKey, processor);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_an_exception_if_a_null_app_key_is_passed_to_constructor()
        {
            new FeedFactory(string.Empty, processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_an_exception_if_an_empty_app_key_is_passed_to_constructor()
        {
            new FeedFactory(null, processor);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_an_exception_if_a_null_processor_is_passed_to_constructor()
        {
            new FeedFactory(TestUtil.AppKey, null);
        }

        [Test]
        [ExpectedException(typeof(WebException))]
        public void Should_use_default_rest_processor_if_only_app_key_is_provided()
        {
            new FeedFactory(TestUtil.AppKey).GetFeed("bad url", 100);
        }

        [Test]
        public void GetFeed_should_use_feed_url()
        {
            feedFactory.GetFeed(URL);
            Assert.AreEqual(URL, processor.GetParameterValue("link"));
        }

        [Test]
        public void GetFeed_should_use_priority()
        {
            feedFactory.GetFeed(URL, 99);
            Assert.AreEqual("99", processor.GetParameterValue("priority"));
        }

        [Test]
        public void GetFeed_should_use_default_priority_of_100_if_not_provided()
        {
            feedFactory.GetFeed(URL);
            Assert.AreEqual("100", processor.GetParameterValue("priority"));
        }

        [Test]
        public void GetFeed_should_return_a_feed_with_the_correct_feed_id()
        {
            processor.Response = string.Format("<results><feed_id>99</feed_id></results>");
            Assert.AreEqual(99, feedFactory.GetFeed(URL).Id);
        }

        [Test]
        [ExpectedException(typeof(PostRankException))]
        public void GetFeed_should_throw_exception_if_error_returned()
        {
            processor.Response = "<results><error>Collecting data</error><progress></progress></results>";
            feedFactory.GetFeed(URL);
        }

        [Test]
        public void GetFeed_with_id_should_simply_create_a_feed()
        {
            processor.ThrowException(new Exception());
            feedFactory.GetFeed(99);
            Assert.AreEqual(99, feedFactory.GetFeed(99).Id);
        }
    }
}