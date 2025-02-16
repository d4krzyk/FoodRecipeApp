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
using AppGUI.ServiceRef1;


namespace AppGUI
{
    /// <summary>
    /// Logika interakcji dla klasy RecipePage.xaml
    /// </summary>
    public partial class RecipePage : Page
    {
        protected string searchQuery = "";
        List<Recipe> recipes = new List<Recipe>();
        public RecipePage()
        {
            InitializeComponent();
            
        }
        public RecipePage(List<Recipe> recipes)
        {
            InitializeComponent();
            this.recipes = recipes;
            DisplayResults(recipes);
        }

        private void onSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchQuery = SearchMealsTextBox.Text;
            Console.WriteLine(searchQuery);
        }

        private void onSearchClicked(object sender, RoutedEventArgs e)
        {
            RecipeLibraryClient client = new RecipeLibraryClient();
            //Console.WriteLine($"Długość nazwy: {searchQuery.Length}");
            Recipe[] results = client.FetchMealsData(searchQuery);
            recipes = results.ToList();
            //foreach (string result in results)
            //{
            //     Console.WriteLine(result);
            //}
            client.Close();
            DisplayResults(recipes);
        }
        private void DisplayResults(List<Recipe> recipes)
        {

            MealsListBox.Items.Clear();
            Style itemStyle = new Style(typeof(ListBoxItem));
            itemStyle.Setters.Add(new Setter(ListBoxItem.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            itemStyle.Setters.Add(new Setter(ListBoxItem.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
            itemStyle.Setters.Add(new Setter(ListBoxItem.BackgroundProperty, new SolidColorBrush(Color.FromRgb(22, 22, 22))));
            MealsListBox.ItemContainerStyle = itemStyle;
            recipes.ForEach(recipe => { MealsListBox.Items.Add(recipe.name); });
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
                    ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new RecipeDetailPage(selectedRecipe, recipes));
                }
            }
        }
    }
}
