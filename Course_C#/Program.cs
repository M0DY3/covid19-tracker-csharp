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
                Console.WriteLine("4. Global Historical Data");
                Console.WriteLine("5. Exit");
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
                        await ShowLatestUpdates(client);
                        break;

                    case "5":
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

        static async Task ShowLatestUpdates(HttpClient client)
        {
            string url = "https://disease.sh/v3/covid-19/historical/all?lastdays=all";
            string response = await client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, long>>>(response);

            Console.WriteLine("\n--- Latest 3 Updates ---");

            foreach (var category in new[] { "cases", "deaths", "recovered" })
            {
                Console.WriteLine($"\n({category})");

                // Sort dates
                var sorted = data[category].OrderBy(entry => DateTime.Parse(entry.Key)).ToList();

                // Take last 4 dates to calculate last 3 updates
                var lastDates = sorted.Skip(sorted.Count - 4).ToList();

                for (int i = 1; i < lastDates.Count; i++)
                {
                    string date = lastDates[i].Key;
                    long update = lastDates[i].Value - lastDates[i - 1].Value;
                    Console.WriteLine($"{date}\t+{update}");
                }
            }
        }

    }



}

