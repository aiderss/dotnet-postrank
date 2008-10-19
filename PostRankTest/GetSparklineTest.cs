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
    public class GetSparklineTest : FeedTest
    {
        public override string RequestFormat
        {
            get { return "png"; }
        }

        public override string RequestMethod
        {
            get { return "sparkline"; }
        }

        public override void SendRequest()
        {
            feed.GetSparkline(Level.All);
        }

        [Test]
        public void Should_return_image_bytes()
        {
            processor.ResponseBytes = new byte[] { 1, 2, 3 };
            byte[] sparkline = feed.GetSparkline(Level.All);

            Assert.AreEqual(new byte[] { 1, 2, 3 }, sparkline);
        }

        [Test]
        public void Should_use_level_as_request_parameter()
        {
            feed.GetSparkline(Level.All);
            Assert.AreEqual("all", processor.GetParameterValue("level"));
        }

        [Test]
        public void Should_use_png_as_image_format()
        {
            feed.GetSparkline(Level.All);
            Assert.AreEqual("png", processor.GetParameterValue("format"));
        }
    }
}