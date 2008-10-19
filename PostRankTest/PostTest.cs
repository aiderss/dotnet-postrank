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
using NUnit.Framework;
using PostRank;

namespace PostRankTest
{
    [TestFixture]
    public class PostTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_should_throw_an_exception_if_null_xml_string_is_passed_in()
        {
            Post.Create((string)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_should_throw_an_exception_if_null_xml_element_is_passed_in()
        {
            Post.Create((XmlElement)null);
        }

        [Test]
        public void Title_property_should_be_the_text_of_the_title_element()
        {
            Post post = Post.Create("<item><title>foo</title></item>");
            Assert.AreEqual("foo", post.Title);
        }

        [Test]
        public void Link_property_should_be_the_text_of_the_link_element()
        {
            Post post = Post.Create("<item><link>http://foo</link></item>");
            Assert.IsInstanceOfType(typeof(Uri), post.Link);
            Assert.AreEqual(new Uri("http://foo"), post.Link);
        }

        [Test]
        public void Guid_property_should_be_the_text_of_the_guid_element()
        {
            Post post = Post.Create("<item><guid>foo</guid></item>");
            Assert.AreEqual("foo", post.Guid);
        }

        [Test]
        public void Source_property_should_be_the_text_of_the_source_element()
        {
            Post post = Post.Create("<item><source>foo</source></item>");
            Assert.AreEqual("foo", post.Source);
        }

        [Test]
        public void SourceUrl_property_should_be_url_attribute_of_the_source_element()
        {
            Post post = Post.Create("<item><source url='http://foo'/></item>");
            Assert.IsInstanceOfType(typeof(Uri), post.SourceUrl);
            Assert.AreEqual(new Uri("http://foo"), post.SourceUrl);
        }

        [Test]
        public void CommentCount_property_should_be_the_text_of_the_slash_comments_element()
        {
            Post post = Post.Create("<item xmlns:slash='yada'><slash:comments>123</slash:comments></item>");
            Assert.AreEqual(123, post.CommentCount);
        }

        [Test]
        public void CommentCount_property_should_be_0_if_slash_comments_element_missing()
        {
            Post post = Post.Create("<item xmlns:slash='yada'></item>");
            Assert.AreEqual(0, post.CommentCount);
        }

        [Test]
        public void PubDate_property_should_be_the_text_of_the_pubDate_element()
        {
            Post post = Post.Create("<item><pubDate>2/16/1992 12:15:12</pubDate></item>");
            Assert.AreEqual(DateTime.Parse("2/16/1992 12:15:12"), post.PubDate);
        }

        [Test]
        public void Description_property_should_be_the_text_of_the_description_element()
        {
            Post post = Post.Create("<item><description>foo</description></item>");
            Assert.AreEqual("foo", post.Description);
        }

        [Test]
        public void Content_property_should_be_the_text_of_the_content_encoded_element()
        {
            Post post = Post.Create("<item xmlns:content='yada'><content:encoded>foo</content:encoded></item>");
            Assert.AreEqual("foo", post.Content);
        }

        [Test]
        public void Postrank_property_should_be_the_text_of_the_postrank_element()
        {
            Post post = Post.Create("<item xmlns:aiderss='yada'><aiderss:postrank>1.0</aiderss:postrank></item>");
            Assert.AreEqual(1.0, post.PostRank);
        }

        [Test]
        public void OriginalLink_property_should_be_the_text_of_the_original_link_element()
        {
            Post post = Post.Create("<item xmlns:aiderss='yada'><aiderss:original_link>http://foo</aiderss:original_link></item>");
            Assert.IsInstanceOfType(typeof(Uri), post.OriginalLink);
            Assert.AreEqual(new Uri("http://foo"), post.OriginalLink);
        }

        [Test]
        public void Color_property_should_be_the_text_of_the_postrank_color_element()
        {
            Post post = Post.Create("<item xmlns:aiderss='yada'><aiderss:postrank_color>#123456</aiderss:postrank_color></item>");
            Assert.AreEqual(Color.FromArgb(0x12, 0x34, 0x56), post.PostRankColor);
        }

        [Test]
        public void Uri_property_should_be_the_text_of_the_uri_element()
        {
            Post post = Post.Create("<item xmlns:aiderss='yada'><aiderss:uri>http://foo</aiderss:uri></item>");
            Assert.IsInstanceOfType(typeof(Uri), post.Uri);
            Assert.AreEqual(new Uri("http://foo"), post.Uri);
        }

        [Test]
        public void This_indexer_should_return_element_text_of_index_parameter()
        {
            Post post = Post.Create("<item><foo>bar</foo></item>");
            Assert.AreEqual("bar", post["foo"]);
        }

        [Test]
        public void This_indexer_should_return_null_for_an_element_that_does_not_exist()
        {
            Post post = Post.Create("<item/>");
            Assert.IsNull(post["foo"]);
        }
    }
}
