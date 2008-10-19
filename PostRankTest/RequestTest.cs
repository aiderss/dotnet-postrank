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
using System.Net;
using NUnit.Framework;
using PostRank;

namespace PostRankTest
{
    [TestFixture]
    public abstract class RequestTest
    {
        protected TestRestProcessor processor;

        public abstract void SendRequest();

        public virtual string RequestFormat
        {
            get { return "rss"; }
        }

        public abstract string RequestMethod
        {
            get;
        }

        [SetUp]
        public virtual void SetUp()
        {
            processor = new TestRestProcessor();
        }

        [Test]
        public void Should_use_api_postrank_com_in_request_host()
        {
            SendRequest();
            Assert.AreEqual("api.postrank.com", Feed.Host);
            Assert.AreEqual(Feed.Host, processor.Request.RequestUri.Host);
        }

        [Test]
        public void Should_use_version_1_in_request_host()
        {
            SendRequest();
            Assert.AreEqual("1", Feed.Version);
            StringAssert.StartsWith("/v1/", processor.Request.RequestUri.LocalPath);
        }

        [Test]
        public void Should_use_method_in_request_path()
        {
            SendRequest();
            StringAssert.EndsWith("/" + RequestMethod, processor.Request.RequestUri.LocalPath);
        }

        [Test]
        public void Should_use_format_as_request_paramter()
        {
            SendRequest();
            Assert.AreEqual(RequestFormat, processor.GetParameterValue("format"));
        }

        [Test]
        public void Should_use_app_key_as_request_paramter()
        {
            SendRequest();
            Assert.AreEqual(TestUtil.AppKey, processor.GetParameterValue("appkey"));
        }

        [Test]
        [ExpectedException(typeof(WebException))]
        public void Should_throw_exception_if_getting_web_response_throws_one()
        {
            processor.ThrowException(new WebException());
            SendRequest();
        }
    }
}