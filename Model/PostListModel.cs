using System;
namespace Telegrambot.Model
{
	public class PostListModel
	{
        public string id { get; set; }
        public string Title { get; set; }
        public int Ups { get; set; }
        public string Url { get; set; }
        public int Commnets { get; set; }
        public DateTime timestamp { get; set; }
    }
}

