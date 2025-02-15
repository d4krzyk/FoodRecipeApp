using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using MealsLibrary1;

namespace AppMealService
{
    public partial class MealService : ServiceBase
    {
        public ServiceHost serviceHost = null;
        private Timer timer;
        //private HttpClient httpClient;
        public MealService()
        {
            InitializeComponent();
            StartServiceLogic();
            //httpClient = new HttpClient();
        }

        protected override void OnStart(string[] args)
        {
            StartServiceLogic();
        }
        public void StartServiceLogic()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            timer = new Timer
            {
                Interval = 60000 // 60 sekund
            };
            //timer.Elapsed += async (sender, e) => await FetchMealDataAsync("carbonara");
            timer.Start();
            // Create a ServiceHost for the WcfCalculatorService type and provide the base address.
            serviceHost = new ServiceHost(typeof(RecipeLibrary));

            // Open the ServiceHostBase to create listeners and start listening for messages.
            serviceHost.Open();
            Console.WriteLine("Usługa została uruchomiona.");
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
            timer?.Stop();
            timer?.Dispose();
            //httpClient?.Dispose();
        }


        
    }
}
