using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace AppMealService
{
    public partial class MealService : ServiceBase
    {
        private Timer timer;
        private HttpClient httpClient;
        public MealService()
        {
            InitializeComponent();
            httpClient = new HttpClient();
        }

        protected override void OnStart(string[] args)
        {
            StartServiceLogic();
        }
        public void StartServiceLogic()
        {
            timer = new Timer
            {
                Interval = 60000 // 60 sekund
            };
            //timer.Elapsed += async (sender, e) => await FetchMealDataAsync("carbonara");
            timer.Start();

            Console.WriteLine("Usługa została uruchomiona.");
        }

        protected override void OnStop()
        {
            timer?.Stop();
            timer?.Dispose();
            httpClient?.Dispose();
        }
        public void TestRun()
        {
            Console.WriteLine("Rozpoczynanie testowej symulacji usługi...");
            StartServiceLogic();

            Task.Run(async () =>
            {
                await FetchMealDataAsync("chicken");
            }).GetAwaiter().GetResult(); // Czeka na zakończenie zapytania

            // Po wykonaniu zapytania zatrzymujemy usługę
            Console.WriteLine("Zakończono pobieranie danych, usługa zostanie zatrzymana.");
            OnStop();

            Console.WriteLine("Usługa została zatrzymana.");
        }

        private async Task FetchMealDataAsync(string meal)
        {
            try
            {
                string apiUrl = $"https://www.themealdb.com/api/json/v1/1/search.php?s={meal}";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{meal}.json");
                    string formattedJson = FormatJson(jsonResult);
                    //File.WriteAllText(filePath, formattedJson);

                    //Console.WriteLine($"Zapisano dane dla potrawy: {meal}");
                    List<string> mealNames = ExtractMealNames(jsonResult);
                    //mealNames.ForEach(Console.WriteLine);
                }
                else
                {
                    Console.WriteLine($"Błąd API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
        private string FormatJson(string json)
        {
            JsonDocument doc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
        }
        private List<string> ExtractMealNames(string jsonResult)
        {
            List<string> mealNames = new List<string>();

            using (JsonDocument doc = JsonDocument.Parse(jsonResult))
            {
                JsonElement root = doc.RootElement;
                JsonElement mealsArray = root.GetProperty("meals");

                if (mealsArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement meal in mealsArray.EnumerateArray())
                    {
                        if (meal.TryGetProperty("strMeal", out JsonElement strMealElement))
                        {
                            mealNames.Add(strMealElement.GetString());
                        }
                    }
                }
            }

            return mealNames;
        }
    }
}
