#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using Assignment_A2_01.Models;
using Assignment_A2_01.ModelsSampleData;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace Assignment_A2_01.Services
{
    public class NewsService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "d318329c40734776a014f9d9513e14ae";
        public async Task<NewsApiData> GetNewsAsync()
        {
            //https://newsapi.org/docs/endpoints/top-headlines
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category=sports&apiKey={apiKey}";
            //UseNewsApiSample      
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();

            //NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync("sports");
            var news = new NewsApiData
            {
                Status = nd.Status,
                TotalResults = nd.TotalResults,
                Articles = nd.Articles.Select(x => new Article
                {
                    Title = x.Title,
                    Source = x.Source,
                    Author = x.Author,
                    Description = x.Description,
                    Url = x.Url
                    ,
                    UrlToImage = x.UrlToImage,
                    PublishedAt = x.PublishedAt,
                    Content = x.Content


                }).ToList()

                
            };
            //foreach (var item in nd.Articles)
            //{
                
            //    System.Console.WriteLine($"{item}");
            //}
            
            
            return news;
        }
    }
}
