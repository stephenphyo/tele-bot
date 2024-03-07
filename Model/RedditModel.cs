using System;
namespace Telegrambot.Model
{
	public class RedditModel
	{
        public class RedditResponse
        {
            public string Kind { get; set; }
            public RedditData Data { get; set; }
        }

        public class RedditData
        {
            public List<RedditPost> Children { get; set; }
        }

        public class RedditPost
        {
            public string Kind { get; set; }
            public RedditPostData Data { get; set; }
        }

        public class RedditPostData
        {
            public string Title { get; set; }
            public int Ups { get; set; }
            public string Url { get; set; }
            public int num_comments { get; set; }
            public double created_utc { get; set; }
        }

        public class Result
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int Ups { get; set; }
            public string Url { get; set; }
            public int Commnets { get; set; }
            public DateTime timestamp { get; set; }

        }
        public class TrendViewModel
        {
            public string Title { get; set; }
            public string Url { get; set; }

            
        }
    }
}

