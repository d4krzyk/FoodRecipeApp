using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppLibrary
{
    internal class RecipeLibrary
    {
        private List<Recipe> _recipes = new List<Recipe>();
        private readonly string _filePath = "cacheLib/meals.json"; // Plik, w którym zapisujesz wyniki

        public RecipeLibrary()
        {
            LoadFromFile();
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
            if (!_recipes.Any(m => m.IdMeal == recipe.IdMeal)) // Unikamy duplikatów
            {
                _recipes.Add(recipe);
                SaveToFile();
            }
        }

        public Recipe GetMealById(string idMeal)
        {
            return _recipes.FirstOrDefault(m => m.IdMeal == idMeal);
        }

        public List<Recipe> GetAllRecipes()
        {
            return _recipes;
        }
    }
}
