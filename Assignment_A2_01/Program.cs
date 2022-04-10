using Assignment_A2_01.Services;
using System;

namespace Assignment_A2_01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Top headlines");
            var news1 = new NewsService().GetNewsAsync();
            news1.Wait();
            var news = news1.Result;

            foreach (var item in news.Articles)
            {
                Console.WriteLine($"{item}");
            }


        }
    }
}
