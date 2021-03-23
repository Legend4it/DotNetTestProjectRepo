using Quartz;
using Quartz.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            //Repository Insert ....

            Console.WriteLine("Start Test Vechile:");
            //Run quartz-scheduler to mok data to db OR Just test data
            // Save test data to db1, and Implement GetData() by Vechile RegNr ....!
            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<PassTimeJob>().UsingJobData("PassTime", DateTime.Now.ToString())
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();
            
            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);

            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(60));

            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }
    }

    public class PassTimeJob : IJob
    {
        public DateTime PassTime { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            PassTime = RandomTime();
            await Console.Out.WriteLineAsync($"{PassTime}:Greetings from HelloJob!");
        }
        private DateTime RandomTime()
        {
            Random gen = new Random();
            DateTime start = DateTime.Now;
            //var range = (DateTime.Today - start).Hours;
            return start.AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
        }
    }
}
