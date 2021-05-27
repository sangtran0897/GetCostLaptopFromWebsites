using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using vtctracking.Lib;
using GetCostLaptopFromWebsites.Models.linq;
using GetCostLaptopFromWebsites.Models;

namespace GetCostLaptopFromWebsites
{
    class Program
    {
        const string tag = "GetCostLaptopFromWebsites";
        static bool isRunning = false;
        static void Main(string[] args)
        {
            Start();
            OnTimedEvent(null, null);

            ConsoleKey key;
            while ((key = Console.ReadKey().Key) != ConsoleKey.Escape)
            {
                if (key == ConsoleKey.F10)
                {                    
                    if (isRunning)
                    {
                        LogAndConsole.LogInfo(tag, "F10 is pressed => isRunning => Don't Reload ProcessLoad");
                        return;
                    }
                    LogAndConsole.LogInfo(tag, "F10 is pressed => Reload ProcessLoad");
                    ProcessLoad();
                }
            }
        }

        public static void Start()
        {
            LoadSetting();
            LogAndConsole.LogInfo(tag, "START_TOOL");

            Timer aTimer = new Timer();
            // Hook up the Elapsed event for the timer. 
            aTimer.Interval = 10000;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (isRunning) return;
            ProcessLoad();
        }
        public static void LoadSetting()
        {

        }
        public static void ProcessLoad()
        {
            LogAndConsole.LogInfo(tag, "ProcessLoad-start");
            isRunning = true;
            var wServices = new List<IWebsiteService>();

            var db = new databaseDataContext();
            var webSites = (from w in db.Websites
                            join s in db.SourceWebsites on w.Id equals s.WebsiteId
                            where w.Id == 1 || w.Id == 2
                            select s).ToList();

            foreach (var item in webSites)
            {
                IWebsiteService smsServiceClient = WebsiteFactory.GetAgency(item);
                wServices.Add(smsServiceClient);
            }

            try
            {
                var tasks = Task.WhenAll(wServices.Select( s =>  s.ProccessGetLaptopPrice()));
                tasks.Wait();
                LogAndConsole.LogInfo(tag, tasks.Status);
                //var inputs = tasks.Where(result => result != true).ToList();
            }
            catch (Exception ex)
            {
                LogAndConsole.LogError(tag, ex);
            }

            isRunning = false;
            LogAndConsole.LogInfo(tag, "ProcessLoad-end");
        }
    }
}
