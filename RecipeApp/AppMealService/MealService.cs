using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using MealsLibrary1;
using System.Configuration;


namespace AppMealService
{
    public partial class MealService : ServiceBase
    {
        public ServiceHost serviceHost = null;
        private Timer timer;
        private EventLog eventLog;
        private readonly string eventLogSource;
        private readonly string eventLogName;
        private readonly double timerInterval;
        public MealService()
        {
            InitializeComponent();
            eventLogSource = ConfigurationManager.AppSettings["EventLogSource"];
            eventLogName = ConfigurationManager.AppSettings["EventLogName"];
            timerInterval = double.Parse(ConfigurationManager.AppSettings["TimerServiceStatusInterval"]);
            SetupEventLog();
        }
        private void SetupEventLog()
        {
            if (!EventLog.SourceExists(eventLogSource))
            {
                EventLog.CreateEventSource("RecipeAppSource", "RecipeAppLog");
            }
            eventLog = new EventLog
            {
                Source = eventLogSource,
                Log = eventLogName
            };
        }
        protected override void OnStart(string[] args)
        {
            StartServiceLogic();
            eventLog.WriteEntry("RecipeService started.", EventLogEntryType.Information);

        }
        public void StartServiceLogic()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            timer = new Timer
            {
                Interval = timerInterval // Interwał odpytywania usługi
            };
            timer.Elapsed += CheckStatusService;
            timer.Start();

            serviceHost = new ServiceHost(typeof(RecipeLibrary));

            serviceHost.Open();
            Console.WriteLine("Usługa została uruchomiona.");
        }

        private void CheckStatusService(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Sprawdzanie statusu usługi...");
            try
            {
                using (ServiceController sc = new ServiceController("RecipeApp-Service"))
                {
                    string status = sc.Status.ToString();
                    eventLog.WriteEntry($"Status usługi: {status}", EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas sprawdzania statusu usługi: {ex.Message}");
            }
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
            eventLog.WriteEntry("RecipeService stopped.", EventLogEntryType.Information);
            timer?.Stop();
            timer?.Dispose();
        }


        
    }
}
