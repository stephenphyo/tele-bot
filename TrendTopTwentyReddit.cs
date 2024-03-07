using System;
using System.Threading;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Telegrambot.Model;
using static Telegrambot.Model.RedditModel;

namespace Telegrambot
{
    public class TrendTopTwentyReddit
    {
        private const string RedditApiUrl = "https://www.reddit.com/r/memes/top.json?limit=20&t=day";
        private readonly HttpClient _client;

        public TrendTopTwentyReddit()
        {
            _client = new HttpClient();
        }
        public async Task<TrendViewModel> GetMemes(long chatId)
        {
            TrendViewModel model = new TrendViewModel();
            List<PostListModel> postList = new List<PostListModel>();
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

                        var posts = redditResponse.Data.Children;
                        var sortedPosts = posts.OrderByDescending(p => p.Data.Ups);
                        int i = 1;
                        foreach (var post in sortedPosts)
                        {
                            var postData = post.Data;


                            // Create  with the extracted fields for each post
                            PostListModel postmodel = new PostListModel()
                            {
                                id = i.ToString(),
                                Url = postData.Url,
                                Title = postData.Title,
                                Ups = postData.Ups,
                                Commnets = postData.num_comments,
                                timestamp = DateTimeOffset.FromUnixTimeSeconds((long)postData.created_utc).DateTime,

                            };
                            i++;

                            postList.Add(postmodel);
                        }

                        byte[] GenerateDocumnet = CreateDocument(postList);
                        String SentPdf = await sendDocument(chatId, GenerateDocumnet);
                        if(SentPdf== "success")
                        {
                            model.Title = "Enjoy it ";
                            model.Url = "not found";
                            return model;
                        }
                        else
                        {
                            model.Title = "Cannot Sent Pdf ";
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

        private async Task<string> sendDocument(long chatId, byte[] generateDocumnet)
        {
            string token = "6303059270:AAG39WQJfEfKtwVoaIWkv-vywsXyfop6CwY";
            string apiUrl = $"https://api.telegram.org/bot{token}/sendDocument";
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);


            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(chatId.ToString()), "chat_id");
            formData.Add(new ByteArrayContent(generateDocumnet), "document", "document.pdf");
            request.Content = formData;

            using (HttpClient client = new HttpClient())
            {

                var response1 = await client.SendAsync(request);


                if (response1.IsSuccessStatusCode)
                {
                    return "success";
                }
                else
                {
                    return "Error";
                }
            }
        }




        public static byte[] CreateDocument(List<PostListModel> postList)
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document(iTextSharp.text.PageSize.LETTER, 0, 0, 0, 0);
            PdfWriter pw = PdfWriter.GetInstance(document, ms);
            document.Open();

            iTextSharp.text.Paragraph Heading = new iTextSharp.text.Paragraph("🔥 Reddit TrendTracker 🔥 ", FontFactory.GetFont("Times New Roman", 25, Font.BOLD, new BaseColor(102, 0, 102)));
            Heading.Alignment = Element.ALIGN_CENTER;
            document.Add(Heading);

            iTextSharp.text.Paragraph trendingParagraph = new iTextSharp.text.Paragraph("Trending Top Twenty Reddit Memes", FontFactory.GetFont("Times New Roman", 20, Font.BOLD, new BaseColor(102, 0, 102)));
            trendingParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(trendingParagraph);

            document.Add(new iTextSharp.text.Paragraph(" "));

            document.Add(new iTextSharp.text.Paragraph(" "));

            PdfPTable table = new PdfPTable(6);

            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.DefaultCell.Padding = 5;


            table.AddCell(CreatCell("No"));
            table.AddCell(CreatCell("Post Name"));
            table.AddCell(CreatCell("Total Votes"));
            table.AddCell(CreatCell("Total Comments"));
            table.AddCell(CreatCell("URL"));
            table.AddCell(CreatCell("Timestamp"));



            foreach (var post in postList)
            {
                table.AddCell(post.id.ToString());
                table.AddCell(post.Title);
                table.AddCell(post.Ups.ToString());
                table.AddCell(post.Commnets.ToString());
                table.AddCell(post.Url.ToString());
                table.AddCell(post.timestamp.ToString("yyyy-MM-dd HH:mm:ss"));

            }


            document.Add(table);

            document.Close();
            byte[] bytesStream = ms.ToArray();
            return bytesStream;
        }

        public static PdfPCell CreatCell(String text)
        {
            var cell = new PdfPCell(new Phrase(text));
            cell.BackgroundColor = new BaseColor(102, 0, 102);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            cell.Phrase = new Phrase(text, FontFactory.GetFont("Times New Roman ", 12, Font.NORMAL, BaseColor.WHITE));
            return cell;
        }
    }
}
    

	

