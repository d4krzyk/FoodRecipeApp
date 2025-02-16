using AppGUI.ServiceRecipeRef;
using MealsLibrary1;
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
        private bool isSaved = false;
        public RecipeDetailPage(Recipe recipe, List<Recipe> recipes, bool isSaved)
        {
            InitializeComponent();
            this.selectedRecipe = recipe;
            this.recipes = recipes;
            this.isSaved = isSaved;
            DisplayRecipeDetails();
            DisplaySaveOptions();
        }
        private void SetStyleButton(Button button,bool isAdd, bool isActive)
        {
            if(!isActive)
            {
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.987));
                brush.GradientStops.Add(new GradientStop(Colors.White, 0.056));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF212121"), 0.848));
                button.Background = brush;
                button.Foreground = new SolidColorBrush(Colors.White);
                button.Opacity = 0.3;
                button.IsEnabled = false;
            }
            else
            {
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.987));
                brush.GradientStops.Add(new GradientStop(Colors.White, 0.056));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF777373"), 0.848));
                button.Background = brush;
                if(isAdd)
                {
                    RadialGradientBrush brushFront = new RadialGradientBrush();
                    brushFront.Center = new Point(0.35, 0.5);
                    brushFront.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFAF7724"), 1));
                    brushFront.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFDCCD27"), 0));
                    brushFront.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFCAAB25"), 0.429));
                    button.Foreground = brushFront;
                }
                else
                {
                    button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC51E1E"));
                }
                button.Opacity = 1;
                button.IsEnabled = true;
            }
            
        }
        private void DisplaySaveOptions()
        {
            SetStyleButton(AddSaveRecipeButton, true, !isSaved);
            SetStyleButton(DelSaveRecipeButton, false, isSaved);
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

        private async void onDelSaveRecipeButtonClicked(object sender, RoutedEventArgs e)
        {
            if (DelSaveRecipeButton.IsEnabled)
            {
                SetStyleButton(DelSaveRecipeButton, false, false);
                RecipeLibraryClient client = new RecipeLibraryClient();
                await Task.Run(() => client.RemoveMealFromSaved(selectedRecipe.idMeal));
                client.Close();
                SetStyleButton(AddSaveRecipeButton, true, true);

            }
        }

        private async void onAddSaveRecipeButtonClicked(object sender, RoutedEventArgs e)
        {
            if (AddSaveRecipeButton.IsEnabled)
            {
                SetStyleButton(AddSaveRecipeButton, true, false);
                RecipeLibraryClient client = new RecipeLibraryClient();
                await Task.Run(() => client.AddMealToSaved(selectedRecipe));
                client.Close();
                SetStyleButton(DelSaveRecipeButton, false, true);
            }
        }
    }
}
