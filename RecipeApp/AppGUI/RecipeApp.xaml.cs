using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppGUI.ServiceRecipeRef;
using MealsLibrary1;


namespace AppGUI
{
    /// <summary>
    /// Logika interakcji dla klasy RecipePage.xaml
    /// </summary>
    public partial class RecipePage : Page
    {
        protected string searchQuery = "";
        List<Recipe> recipes = new List<Recipe>();
        List<Recipe> savedRecipes = new List<Recipe>();
        public RecipePage()
        {
            InitializeComponent();
            RecipeLibraryClient client = new RecipeLibraryClient();
            Recipe[] allSavedMeals_temp = client.GetAllSavedRecipes();
            savedRecipes = allSavedMeals_temp.ToList();
            SearchResultLabel.Content = "";
            DisplayResults(savedRecipes, SavedMealsListBox);

        }
        public RecipePage(List<Recipe> recipes)
        {
            InitializeComponent();
            this.recipes = recipes;
            RecipeLibraryClient client = new RecipeLibraryClient();
            Recipe[] allSavedMeals_temp = client.GetAllSavedRecipes();
            savedRecipes = allSavedMeals_temp.ToList();
            SearchResultLabel.Content = "";
            DisplayResults(savedRecipes, SavedMealsListBox);
            DisplayResults(recipes, MealsListBox);
        }
        private bool isSaved(Recipe recipe)
        {
            return savedRecipes.Any(r => r.idMeal == recipe.idMeal);
        }

        private void onSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchQuery = SearchMealsTextBox.Text;
            Console.WriteLine(searchQuery);
        }

        public async void onSearchClicked(object sender, RoutedEventArgs e)
        {
            RecipeLibraryClient client = new RecipeLibraryClient();
            //Console.WriteLine($"Długość nazwy: {searchQuery.Length}");
            Recipe[] results = await Task.Run(() => client.FetchMealsData(searchQuery));
            if(!(results.Length < 1))
            {
                recipes = results.ToList();
                DisplayResults(recipes, MealsListBox);
                SearchResultLabel.Content = $"Search result: found {recipes.Count} recipes";
            }
            else
            {
                SearchResultLabel.Content = $"Search result: No recipes found :(";
            }
            client.Close();
            client.Close();
            DisplayResults(recipes,MealsListBox);
        }
        private void DisplayResults(List<Recipe> recipes, ListBox listbox)
        {

            listbox.Items.Clear();
            Style itemStyle = new Style(typeof(ListBoxItem));
            itemStyle.Setters.Add(new Setter(ListBoxItem.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            itemStyle.Setters.Add(new Setter(ListBoxItem.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
            itemStyle.Setters.Add(new Setter(ListBoxItem.BackgroundProperty, new SolidColorBrush(Color.FromRgb(22, 22, 22))));
            listbox.ItemContainerStyle = itemStyle;
            recipes.ForEach(recipe => { listbox.Items.Add(recipe.name); });
        }

        private void onMealsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MealsListBox.SelectedItem != null)
            {
                // Pobierz wybrany przepis
                string selectedRecipeName = MealsListBox.SelectedItem.ToString();
                Recipe selectedRecipe = recipes.FirstOrDefault(r => r.name == selectedRecipeName);

                if (selectedRecipe != null)
                {
                    // Przejdź do nowej strony z identyfikatorem przepisu
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new RecipeDetailPage(selectedRecipe, recipes, isSaved(selectedRecipe)));
                }
            }
        }

        private void onSavedMealsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SavedMealsListBox.SelectedItem != null)
            {
                // Pobierz wybrany przepis
                string selectedRecipeName = SavedMealsListBox.SelectedItem.ToString();
                Recipe selectedRecipe = savedRecipes.FirstOrDefault(r => r.name == selectedRecipeName);

                if (selectedRecipe != null)
                {
                    // Przejdź do nowej strony z identyfikatorem przepisu
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new RecipeDetailPage(selectedRecipe, recipes, isSaved(selectedRecipe)));
                }
            }
        }
    }
}
