using AppGUI.ServiceRef1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AppGUI
{
    /// <summary>
    /// Logika interakcji dla klasy RecipeDetailPage.xaml
    /// </summary>
    public partial class RecipeDetailPage : Page
    {
        private Recipe selectedRecipe;
        private List<Recipe> recipes;
        public RecipeDetailPage(Recipe recipe, List<Recipe> recipes)
        {
            InitializeComponent();
            selectedRecipe = recipe;
            this.recipes = recipes;
            DisplayRecipeDetails();
        }
        private void DisplayRecipeDetails()
        {
            //Console.WriteLine(selectedRecipe.instructions);
            InstructionTextBox.Text = selectedRecipe.instructions;
            MealNameLabel.Text = selectedRecipe.name;
            CategoryMealTextBox.Text = "Category: \n" + selectedRecipe.category;
            AreaMealTextBox.Text = "Area: \n" + selectedRecipe.area;
            // Ustaw obraz z URL
            if (!string.IsNullOrEmpty(selectedRecipe.imageUrl))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                Console.WriteLine(selectedRecipe.imageUrl);
                bitmap.UriSource = new Uri(selectedRecipe.imageUrl, UriKind.Absolute);
                bitmap.EndInit();
                ThumbImageBox.Source = bitmap;
            }
        }

        private void onYoutubeLinkClicked(object sender, RoutedEventArgs e)
        {
            string url = selectedRecipe.youtubeUrl;
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void onSourceLinkClicked(object sender, RoutedEventArgs e)
        {
            string url = selectedRecipe.sourceUrl;
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void onBackButtonClicked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new RecipePage(recipes));
        }
    }
}
