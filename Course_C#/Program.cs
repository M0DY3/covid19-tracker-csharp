using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CovidTrackerSimple
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string input;

            while (true)
            {
                Console.WriteLine("\n[+] COVID-19 Tracker [+]");
                Console.WriteLine("1. Global Stats");
                Console.WriteLine("2. Country Stats");
                Console.WriteLine("3. Compare Two Countries");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ShowGlobalStats(client);
                        break;

                    case "2":
                        await ShowCountryStats(client);
                        break;

                    case "3":
                        await CompareCountries(client);
                        break;

                    case "4":
                        Console.WriteLine("Exiting program...");
                        return;

                    default:
                        Console.WriteLine("Invalid input! Please try again.");
                        break;
                }
            }
        }

        static async Task ShowGlobalStats(HttpClient client)
        {
            string url = "https://disease.sh/v3/covid-19/all";
            string response = await client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

            Console.WriteLine("\n--- Global Stats ---");
            Console.WriteLine($"Cases: {data["cases"]}");
            Console.WriteLine($"Deaths: {data["deaths"]}");
            Console.WriteLine($"Recovered: {data["recovered"]}");
        }

        static async Task ShowCountryStats(HttpClient client)
        {
            Console.Write("\nEnter country name: ");
            string country = Console.ReadLine();
            string url = $"https://disease.sh/v3/covid-19/countries/{country}";
            string response = await client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

            Console.WriteLine($"\n--- Stats for {country} ---");
            Console.WriteLine($"Cases: {data["cases"]}");
            Console.WriteLine($"Deaths: {data["deaths"]}");
            Console.WriteLine($"Recovered: {data["recovered"]}");
        }

        static async Task CompareCountries(HttpClient client)
        {
            Console.Write("\nEnter first country: ");
            string country1 = Console.ReadLine();
            string url1 = $"https://disease.sh/v3/covid-19/countries/{country1}";
            string response1 = await client.GetStringAsync(url1);
            var data1 = JsonSerializer.Deserialize<Dictionary<string, object>>(response1);

            Console.Write("Enter second country: ");
            string country2 = Console.ReadLine();
            string url2 = $"https://disease.sh/v3/covid-19/countries/{country2}";
            string response2 = await client.GetStringAsync(url2);
            var data2 = JsonSerializer.Deserialize<Dictionary<string, object>>(response2);

            if (country1.ToLower() == country2.ToLower())
            {
                Console.WriteLine("Both countries are the same. Try again.");
                return;
            }

            Console.WriteLine("\n--- Comparison ---");
            Console.WriteLine($"{country1} - Cases: {data1["cases"]}, Deaths: {data1["deaths"]}, Recovered: {data1["recovered"]}");
            Console.WriteLine($"{country2} - Cases: {data2["cases"]}, Deaths: {data2["deaths"]}, Recovered: {data2["recovered"]}");
        }
    }
}
