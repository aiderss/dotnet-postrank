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
using PostRank;

namespace PostRankTest
{
    public class TestRestProcessor : RequestProcessor
    {
        private WebRequest request;
        private string response = "<rss><channel/></rss>";
        private string postContents = null;
        private byte[] responseBytes = null;
        private Exception exception;

        public WebRequest Request
        {
            get { return request; }
        }

        public string PostContents
        {
            get { return postContents; }
        }

        public string Response
        {
            get { return response; }
            set { response = value; }
        }

        public byte[] ResponseBytes
        {
            get { return responseBytes; }
            set { responseBytes = value; }
        }

        public override string GetResponse(string message, string postContents)
        {
            CreateRequestAndOptionallyThrowException(message, postContents);

            return response;
        }

        public override byte[] GetResponseAsBytes(string message, string postContents)
        {
            CreateRequestAndOptionallyThrowException(message, postContents);

            return responseBytes;
        }

        public string GetParameterValue(string parameterName)
        {
            string[] queries = Request.RequestUri.Query.Substring(1).Split(new char[] { '&' });

            foreach(string query in queries)
            {
                string[] nameValue = query.Split(new char[] {'='});
                if (nameValue[0] == parameterName)
                    return nameValue[1];
            }

            return null;
        }

        public void ThrowException(Exception x)
        {
            exception = x;
        }

        private void CreateRequestAndOptionallyThrowException(string requestString, string postContents)
        {
            request = WebRequest.Create(requestString);
            this.postContents = postContents;

            if (exception != null)
                throw exception;
        }
    }
}