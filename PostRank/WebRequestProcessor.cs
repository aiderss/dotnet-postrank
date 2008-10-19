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
using System.IO;
using System.Net;
using System.Text;

namespace PostRank
{
    public class WebRequestProcessor : RequestProcessor
    {
        public override string GetResponse(string message, string postContents)
        {
            using (WebResponse response = GetResponseTo(message, postContents))
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public override byte[] GetResponseAsBytes(string message, string postContents)
        {
            using (WebResponse response = GetResponseTo(message, postContents))
            {
                const int BufferSize = 1024;
                byte[] responseData = new byte[0];
                byte[] buffer = new byte[BufferSize];

                using (BinaryReader reader = new BinaryReader(response.GetResponseStream()))
                {
                    int count = reader.Read(buffer, 0, BufferSize);

                    while (count > 0)
                    {
                        Array.Resize(ref responseData, responseData.Length + count);
                        Array.Copy(buffer, 0, responseData, responseData.Length - count, count);
                        count = reader.Read(buffer, 0, BufferSize);
                    }
                }

                return responseData;
            }
        }

        private static WebResponse GetResponseTo(string message, string postContents)
        {
            WebRequest request = WebRequest.Create(message);

            if (postContents != null)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postContents.Length;

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
                {
                    writer.Write(postContents);
                    writer.Close();
                }
            }

            return request.GetResponse();
        }
    }
}