using AppLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AppMealService
{
    internal static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        static void Main()
        {
#if DEBUG
            // Debugowanie w postaci aplikacji konsolowej
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MealService()
            };
            ServiceBase.Run(ServicesToRun);
            using (ServiceHost host = new ServiceHost(typeof(RecipeLibrary)))
            {
                host.Open();
                Console.WriteLine("Usługa uruchomiona. Naciśnij [Enter], aby zakończyć...");
                Console.ReadLine();
            }
            
#else
            // Normalne uruchomienie jako usługa Windows

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MealService()
            };
            ServiceBase.Run(ServicesToRun);
            using (ServiceHost host = new ServiceHost(typeof(RecipeLibrary)))
            {
                host.Open();
                Console.WriteLine("Usługa uruchomiona. Naciśnij [Enter], aby zakończyć...");
                Console.ReadLine();
            }
#endif
        }
    }
}
