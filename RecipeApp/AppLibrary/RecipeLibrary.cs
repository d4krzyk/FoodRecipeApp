using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppLibrary
{
    public class RecipeLibrary : IRecipeLibrary
    {
        private List<Recipe> _recipes = new List<Recipe>();
        private readonly string _filePath = "cacheLib/meals.json"; // Plik, w którym zapisujesz wyniki
        private HttpClient httpClient;
        public RecipeLibrary()
        {
            LoadFromFile();
            httpClient = new HttpClient();
        }

        private void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _recipes = JsonSerializer.Deserialize<List<Recipe>>(json) ?? new List<Recipe>();
            }
        }

        public void SaveToFile()
        {
            string json = JsonSerializer.Serialize(_recipes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void AddMeal(Recipe recipe)
        {
            if (!_recipes.Any(m => m.idMeal == recipe.idMeal)) // Unikamy duplikatów
            {
                _recipes.Add(recipe);
                SaveToFile();
            }
        }

        public Recipe GetMealById(string idMeal)
        {
            return _recipes.FirstOrDefault(m => m.idMeal == idMeal);
        }

        public List<Recipe> GetAllRecipes()
        {
            return _recipes;
        }

        public async Task<List<string>> FetchMealDataAsync(string meal)
        {
            List<string> mealNames;
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
                    mealNames = ExtractMealNames(jsonResult);
                    //mealNames.ForEach(Console.WriteLine);
                    return mealNames;
                }
                else
                {
                    Console.WriteLine($"Błąd API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
                return null;
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
