using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Configuration;
namespace MealsLibrary1
{
    public class RecipeLibrary : IRecipeLibrary
    {
        private List<Recipe> _savedRecipes = new List<Recipe>();
        private static readonly string AppDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //private static readonly string _filePath = Path.Combine(AppDirectory,"cacheLib","saved-meals.json");
        private readonly string _filePath;
        private readonly HttpClient httpClient;
        private EventLog eventLog;
        private readonly string apiUrl;
        private readonly string eventLogSource;
        private readonly string eventLogName;

        public RecipeLibrary()
        {
            Console.WriteLine($"AppDirectory: {AppDirectory}");
            _filePath = Path.Combine(AppDirectory, ConfigurationManager.AppSettings["FilePath"]);
            string directoryPath = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
            eventLogSource = ConfigurationManager.AppSettings["EventLogSource"];
            eventLogName = ConfigurationManager.AppSettings["EventLogName"];

            LoadFromFile();
            httpClient = new HttpClient();
            SetupEventLog();
        }
        private void SetupEventLog()
        {
            if (!EventLog.SourceExists(eventLogSource))
            {
                EventLog.CreateEventSource("RecipeAppSource", "RecipeAppLog");
            }
            eventLog = new EventLog
            {
                Source = eventLogSource,
                Log = eventLogName
            };
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
                Console.WriteLine($"Błąd podczas ładowania pliku: {ex.Message}");
                throw;
            }
        }

        public void SaveToFile()
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string json = JsonSerializer.Serialize(_savedRecipes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                eventLog.WriteEntry("Saved recipes to file.", EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
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
                    eventLog.WriteEntry($"Added recipe {recipe.name} to saved recipes.", EventLogEntryType.Information);

                }
            }
            catch (Exception ex)
            {
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
                    eventLog.WriteEntry($"Removed recipe {recipeToRemove.name} from saved recipes.", EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {

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
            if (meal == null)
            {
                throw new ArgumentNullException(nameof(meal), "Search query cannot be null.");
            }
            List<Recipe> recipes;
            try
            {
                string APIUrl = $"{apiUrl}{meal}";
                //string APIUrl = $"https://www.themealdb.com/api/json/v1/1/search.php?s={meal}";
                HttpResponseMessage response = httpClient.GetAsync(APIUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = response.Content.ReadAsStringAsync().Result;
                    recipes = ExtractRecipes(jsonResult);
                    eventLog.WriteEntry($"Fetched {recipes.Count} recipes for meal {meal}.", EventLogEntryType.Information);
                    return recipes;
                }
                else
                {
                    eventLog.WriteEntry($"API error: {response.StatusCode}", EventLogEntryType.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry($"Error: {ex.Message}", EventLogEntryType.Error);
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
