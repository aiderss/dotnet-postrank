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
using System.Collections.Generic;
using PostRank;

namespace PostRankTest
{
    class SampleApp
    {

        public static void Main(string[] args)
        {
            ShowInfo(args.Length == 0 ? "http://igvita.com/" : args[0]);
        }

        private static void ShowInfo(string url)
        {
            Feed feed = new FeedFactory(TestUtil.AppKey).GetFeed(url);

            Console.WriteLine("PostRank data for " + feed.Link);
            Console.WriteLine();

            Console.WriteLine("Feed properties:");
            Console.WriteLine("  Title:         " + feed.Title);
            Console.WriteLine("  Description:   " + feed.Description);
            Console.WriteLine("  Id:            " + feed.Id);
            Console.WriteLine("  PubDate:       " + feed.PubDate);
            Console.WriteLine("  Ttl:           " + feed.Ttl);
            Console.WriteLine("  StartIndex:    " + feed.StartIndex);
            Console.WriteLine("  ItemsPerPage:  " + feed.ItemsPerPage);
            Console.WriteLine();

            WritePosts("Top 5 posts in the last year (ordered by PostRank, date):",
                       feed.GetTopPosts(Period.Year, 5));

            IEnumerable<Post> mostRecent5GoodPosts = feed.GetPosts(Level.Good, 5, 0);
            WritePosts("Most recent 5 good posts:", mostRecent5GoodPosts);

            WritePosts("Next most recent 5 good posts:",
                       feed.GetPosts(Level.Good, 5, 5));

            List<Uri> mostRecent5GoodUris = new List<Uri>();
            foreach (Post post in mostRecent5GoodPosts)
            {
                mostRecent5GoodUris.Add(post.OriginalLink);
            }

            WritePosts("Thematic PostRank of most recent 5 good posts:",
                       feed.GetPostRank(mostRecent5GoodUris, null));

            WritePosts("Feed-Based PostRank of most recent 5 good posts:",
                       feed.GetPostRank(mostRecent5GoodUris, new Feed[] {feed}));
        }

        private static void WritePosts(string title, IEnumerable<Post> posts)
        {
            Console.WriteLine(title);
            foreach (Post post in posts)
            {
                Console.Write("  {0,4:#0.0}", post.PostRank);
                if (post.PubDate > DateTime.MinValue)
                    Console.Write(" {0,10:d}", post.PubDate);
                Console.WriteLine(" {0}", post.Title == null ? post.OriginalLink.ToString() : post.Title);
            }
            Console.WriteLine();
        }
    }

}
    