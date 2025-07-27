using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
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

namespace AppGUI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceController controller1 = new ServiceController("RecipeApp-Service");
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            MainFrame.Navigate(new WelcomePage());
        }
        
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            StopService();
        }

        private void StopService()
        {
            try
            {
                if (controller1.Status != ServiceControllerStatus.Stopped && controller1.Status != ServiceControllerStatus.StopPending)
                {
                    controller1.Stop();
                    controller1.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zatrzymywania usługi: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
