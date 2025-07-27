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
using System.Windows.Shapes;
using System.ServiceProcess;
using System.Security.Principal;
namespace AppGUI
{
    /// <summary>
    /// Logika interakcji dla klasy WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        ServiceController controller1 = new ServiceController("RecipeApp-Service");

        public WelcomePage()
        {
            InitializeComponent();
            
        }


        private async void onStartButton(object sender, RoutedEventArgs e)
        {
            ServiceLoaderBar.Visibility = Visibility.Visible;
            try
            {
                await Task.Run(() =>
                {
                    controller1.Refresh();
                    if (controller1.Status == ServiceControllerStatus.Running)
                    {
                        controller1.Stop();
                        controller1.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                    controller1.Start();
                    controller1.WaitForStatus(ServiceControllerStatus.Running);
                });
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new RecipePage());

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas uruchamiania usługi: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ServiceLoaderBar.Visibility = Visibility.Hidden;
            }
        }
    }
}
