﻿using System;
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

namespace AppGUI
{
    /// <summary>
    /// Logika interakcji dla klasy RecipePage.xaml
    /// </summary>
    public partial class RecipePage : Page
    {
        protected string searchQuery = "";
        public RecipePage()
        {
            InitializeComponent();
        }

        private void onSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchQuery = SearchMealsTextBox.Text;
            Console.WriteLine(searchQuery);
        }

        private void onSearchClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
