using AppGUI;
using MealsLibrary1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using AppGUI.ServiceRecipeRef;
using System.Linq;
using System.ServiceProcess;
using System.Diagnostics;

namespace AppGUITests
{
    [TestClass]
    public class RecipeDetailPageTests
    {
        private ServiceController serviceController;

        [TestInitialize]
        public void TestInitialize()
        {
            serviceController = new ServiceController("RecipeApp-Service");
            if (serviceController.Status != ServiceControllerStatus.Running)
            {
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (serviceController.Status == ServiceControllerStatus.Running)
            {
                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
            }
        }

        [TestMethod()]
        public void DisplayRecipeDetailsTest()
        {
            // Arrange
            var recipe = new Recipe
            {
                idMeal = "1",
                name = "Test Recipe",
                category = "Test Category",
                area = "Test Area",
                instructions = "Test Instructions",
                imageUrl = "",
                sourceUrl = "",
                youtubeUrl = "",
            };
            var recipes = new List<Recipe> { recipe };
            var page = new RecipeDetailPage(recipe, recipes, false);

            // Act
            page.DisplayRecipeDetails();

            // Assert
            Assert.AreEqual("Test Instructions", page.InstructionTextBox.Text, "Instructions should be set correctly.");
            Assert.AreEqual("Test Recipe", page.MealNameLabel.Text, "Meal name should be set correctly.");
            Assert.AreEqual("Category: \nTest Category", page.CategoryMealTextBox.Text, "Category should be set correctly.");
            Assert.AreEqual("Area: \nTest Area", page.AreaMealTextBox.Text, "Area should be set correctly.");
            Assert.IsNotNull(page.selectedRecipe.sourceUrl, "Web source should not be null."); 
            Assert.IsNotNull(page.selectedRecipe.youtubeUrl, "Youtube source should not be null.");
            Assert.IsNotNull(page.selectedRecipe.imageUrl, "Image source should not be null.");
        }

        [TestMethod()]
        public void AddMealToSavedTest_NoDuplicates()
        {
            // Arrange
            var recipe = new Recipe
            {
                idMeal = "-1",
                name = "Test Recipe1",
                category = "Test Category",
                area = "Test Area",
                instructions = "Test Instructions",
                imageUrl = "https://www.example.com/test.jpg",
                sourceUrl = "",
                youtubeUrl = "",
            };
            RecipeLibraryClient client = new RecipeLibraryClient();
            

            Recipe[] savedRecipes_pre = client.GetAllSavedRecipes();
            client.AddMealToSaved(recipe);
            // Act
            client.AddMealToSaved(recipe); // Próbujemy dodać ten sam przepis ponownie
            Recipe[] savedRecipes_post = client.GetAllSavedRecipes();
            //Clean test meal recipe
            client.RemoveMealFromSaved(recipe.idMeal);
            // Assert
            Assert.AreEqual(savedRecipes_pre.Length + 1, savedRecipes_post.Length, "There should be only one saved recipe.");
            Assert.IsTrue(savedRecipes_post.Any(r => r.idMeal == "-1"), "Saved recipes should contain the recipe.");  
        }

        [TestMethod()]
        public void RemoveMealFromSavedTest()
        {
            // Arrange
            var recipe = new Recipe
            {
                idMeal = "-2",
                name = "Test Recipe2",
                category = "Test Category",
                area = "Test Area",
                instructions = "Test Instructions",
                imageUrl = "https://www.example.com/test.jpg",
                sourceUrl = "",
                youtubeUrl = "",
            };
            RecipeLibraryClient client = new RecipeLibraryClient();
            Recipe[] savedRecipes_pre = client.GetAllSavedRecipes();
            client.AddMealToSaved(recipe);

            // Act
            client.RemoveMealFromSaved(recipe.idMeal);
            Recipe[] savedRecipes_post = client.GetAllSavedRecipes();

            // Assert
            Assert.AreEqual(savedRecipes_pre.Length, savedRecipes_post.Length, "There should be the same amounth of recipes before and after add and remove meal.");
            Assert.IsFalse(savedRecipes_post.Any(r => r.idMeal == "-2"), "Saved recipes should not contain the recipe.");
        }
    }
}
