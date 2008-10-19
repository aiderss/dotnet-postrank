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
using System.Collections;
using System.Collections.Generic;
using PostRank;

namespace PostRankTest
{
    internal static class TestUtil
    {
        public static readonly string AppKey = "test.dpwhelan.com";
        public static readonly int FeedId = 1;

        internal static int GetPostCount(IEnumerable<Post> posts)
        {
            int count = 0;
            IEnumerator<Post> enumerator = posts.GetEnumerator();

            while (enumerator.MoveNext())
                count++;

            return count;
        }

        internal static Post GetFirstPost(IEnumerable<Post>posts)
        {
            IEnumerator enumerator = posts.GetEnumerator();
            enumerator.MoveNext();
            return (Post) enumerator.Current;
        }
    }
}
