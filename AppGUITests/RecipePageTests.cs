using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealsLibrary1;
using AppGUI.ServiceRecipeRef;
using System.ServiceModel;
using System.ServiceProcess;

namespace AppGUI.Tests
{
    [TestClass()]
    public class RecipePageTests
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
        public async Task onSearchClickedTest()
        {
            string searchQuery = "chicken"; // Przykładowe zapytanie
            List<Recipe> recipes = new List<Recipe>();
            RecipeLibraryClient client = new RecipeLibraryClient();

            Recipe[] results = await Task.Run(() => client.FetchMealsData(searchQuery));

            // Assert
            Assert.IsNotNull(results, "Results should not be null");
            Assert.IsTrue(results.Length > 0, "Results should contain at least one item");
            Assert.IsInstanceOfType(results, typeof(Recipe[]), "Results should be of type Recipe[]");
            Assert.IsTrue(results.Any(r => r.name.Contains("Chicken")), "Results should contain a recipe with 'Chicken' in the name");
            Assert.AreEqual("Chicken Handi", results[0].name, "The first recipe name should be 'Chicken Handi'");

        }
        [TestMethod()]
        public async Task FetchMealsDataTest_NoResults()
        {
            // Arrange
            string searchQuery = "nonexistentmeal"; // Zapytanie, które nie zwróci wyników
            RecipeLibraryClient client = new RecipeLibraryClient();

            // Act
            Recipe[] results = await Task.Run(() => client.FetchMealsData(searchQuery));

            // Assert
            Assert.IsNotNull(results, "Results should not be null");
            Assert.AreEqual(0, results.Length, "Results should be empty for nonexistent meal.");
        }
        [TestMethod]
        public async Task onSearchClickedTest_ThrowsArgumentNullException()
        {
            string searchQuery = null; // Puste zapytanie
            RecipeLibraryClient client = new RecipeLibraryClient();
            try
            {
                Recipe[] results = await Task.Run(() => client.FetchMealsData(searchQuery));
                Assert.Fail("Expected FaultException<ExceptionDetail> was not thrown.");
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                // Assert
                string expectedMessage = "Search query cannot be null.";
                StringAssert.Contains(ex.Detail.Message, expectedMessage);
            }
        }
    }
}