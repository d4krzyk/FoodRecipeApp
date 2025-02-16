using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MealsLibrary1
{
    public class RecipeLibrary : IRecipeLibrary
    {
        private List<Recipe> _savedRecipes = new List<Recipe>();
        private static readonly string AppDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _filePath = Path.Combine(AppDirectory, "cacheLib", "saved-meals.json");
        private HttpClient httpClient;
        public RecipeLibrary()
        {
            Console.WriteLine($"AppDirectory: {AppDirectory}");
            // Upewnij się, że katalog istnieje
            string directoryPath = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            LoadFromFile();
            httpClient = new HttpClient();
        }

        private void LoadFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _savedRecipes = JsonSerializer.Deserialize<List<Recipe>>(json) ?? new List<Recipe>();
                }
            }
            catch (Exception ex)
            {
                // Logowanie błędu
                Console.WriteLine($"Błąd podczas ładowania pliku: {ex.Message}");
                throw;
            }
        }

        public void SaveToFile()
        {
            try
            {
                // Upewnij się, że katalog istnieje
                string directoryPath = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Zapisz plik
                string json = JsonSerializer.Serialize(_savedRecipes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                // Logowanie błędu
                Console.WriteLine($"Błąd podczas zapisywania pliku: {ex.Message}");
                throw;
            }
        }

        public void AddMealToSaved(Recipe recipe)
        {
            try
            {
                if (!_savedRecipes.Any(m => m.idMeal == recipe.idMeal)) // Unikamy duplikatów
                {
                    _savedRecipes.Add(recipe);
                    SaveToFile();
                }
            }
            catch (Exception ex)
            {
                // Logowanie błędu
                Console.WriteLine($"Błąd podczas dodawania przepisu: {ex.Message}");
                throw;
            }
        }
        public void RemoveMealFromSaved(string idMeal)
        {
            try
            {
                Recipe recipeToRemove = _savedRecipes.FirstOrDefault(m => m.idMeal == idMeal);
                if (recipeToRemove != null)
                {
                    _savedRecipes.Remove(recipeToRemove);
                    SaveToFile();
                }
            }
            catch (Exception ex)
            {
                // Logowanie błędu
                Console.WriteLine($"Błąd podczas usuwania przepisu: {ex.Message}");
                throw;
            }
        }
        public List<Recipe> GetAllSavedRecipes()
        {
            LoadFromFile();
            return _savedRecipes;
        }

        public List<Recipe> FetchMealsData(string meal)
        {
            List<Recipe> recipes;
            try
            {
                string typesearch = "s";
                string apiUrl = $"https://www.themealdb.com/api/json/v1/1/search.php?{typesearch}={meal}";
                HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = response.Content.ReadAsStringAsync().Result;
                    recipes = ExtractRecipes(jsonResult);
                    return recipes;
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

        private List<Recipe> ExtractRecipes(string jsonResult)
        {
            List<Recipe> recipes = new List<Recipe>();

            using (JsonDocument doc = JsonDocument.Parse(jsonResult))
            {
                JsonElement root = doc.RootElement;
                JsonElement mealsArray = root.GetProperty("meals");

                if (mealsArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement meal in mealsArray.EnumerateArray())
                    {
                        Recipe recipe = JsonSerializer.Deserialize<Recipe>(meal.GetRawText());
                        recipes.Add(recipe);
                    }
                }
            }

            return recipes;
        }
    
    }
}
