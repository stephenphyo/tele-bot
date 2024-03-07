using System;
using System.Reflection;
using static Telegrambot.Model.RedditModel;

namespace Telegrambot
{
	public class TrendingReddit
	{
        private const string RedditApiUrl = "https://www.reddit.com/r/memes/top.json?limit=1&t=day";
        private const string RedditApiUrlWithOneMonth = "https://www.reddit.com/r/memes/top.json?limit=1&t=month";
        private const string RedditApiUrlWithOneYear = "https://www.reddit.com/r/memes/top.json?limit=1&t=year";


        public async Task<TrendViewModel> GetMemes()
        {
            TrendViewModel model = new TrendViewModel();
            try
            {
                using (var client = new HttpClient())
                {
                    
                    client.DefaultRequestHeaders.Add("User-Agent", "DataMining"); // Reddit API requires a User-Agent header
                    var response = await client.GetAsync(RedditApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();


                        var redditResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RedditResponse>(content);

                        var posts = redditResponse.Data.Children.Select(child => child.Data);
                        var sortedPosts = posts.OrderByDescending(p => p.Ups).FirstOrDefault();

                        if (sortedPosts != null)
                        {
                            
                            model.Title = sortedPosts.Title;
                            model.Url = sortedPosts.Url;
                            return model; 
                        }
                        else
                        {
                            model.Title = "no title";
                            model.Url = "not found";
                            return model;
                        }
                    }
                    else
                    {
                        model.Title = "Trend Post cannot be retrieved";
                        model.Url = "not found";
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                model.Title = ex.Message;
                model.Url = "not found";
                return model;
            }
        }

        public async Task<TrendViewModel> GetMemesWithinOneMonth()
        {
            TrendViewModel model = new TrendViewModel();
            try
            {
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Add("User-Agent", "DataMining"); // Reddit API requires a User-Agent header
                    var response = await client.GetAsync(RedditApiUrlWithOneMonth);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();


                        var redditResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RedditResponse>(content);

                        var posts = redditResponse.Data.Children.Select(child => child.Data);
                        var sortedPosts = posts.OrderByDescending(p => p.Ups).FirstOrDefault();

                        if (sortedPosts != null)
                        {

                            model.Title = sortedPosts.Title;
                            model.Url = sortedPosts.Url;
                            return model;
                        }
                        else
                        {
                            model.Title = "no title";
                            model.Url = "not found";
                            return model;
                        }
                    }
                    else
                    {
                        model.Title = "Trend Post cannot be retrieved";
                        model.Url = "not found";
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                model.Title = ex.Message;
                model.Url = "not found";
                return model;
            }
        }

        public async Task<TrendViewModel> GetMemesWithinOneYear()
        {
            TrendViewModel model = new TrendViewModel();
            try
            {
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Add("User-Agent", "DataMining");
                    var response = await client.GetAsync(RedditApiUrlWithOneYear);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();


                        var redditResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<RedditResponse>(content);

                        var posts = redditResponse.Data.Children.Select(child => child.Data);
                        var sortedPosts = posts.OrderByDescending(p => p.Ups).FirstOrDefault();

                        if (sortedPosts != null)
                        {

                            model.Title = sortedPosts.Title;
                            model.Url = sortedPosts.Url;
                            return model;
                        }
                        else
                        {
                            model.Title = "no title";
                            model.Url = "not found";
                            return model;
                        }
                    }
                    else
                    {
                        model.Title = "Trend Post cannot be retrieved";
                        model.Url = "not found";
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                model.Title = ex.Message;
                model.Url = "not found";
                return model;
            }
        }
    }
}


