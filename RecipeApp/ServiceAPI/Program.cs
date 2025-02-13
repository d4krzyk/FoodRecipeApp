using System;
using System.Collections.Generic;
using System.Linq;
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
            MealService service = new MealService();
            service.TestRun();
            #else
            // Normalne uruchomienie jako usługa Windows
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MealService()
            };
            ServiceBase.Run(ServicesToRun);
            #endif
        }
    }
}
