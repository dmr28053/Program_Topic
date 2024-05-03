using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Program_Topic
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<SimpleClock>()
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            Console.WriteLine("This program outputs the time every 10 seconds. Press any key to exit.");
            await scheduler.ScheduleJob(job, trigger);

            Console.ReadKey();
        }

        public class SimpleClock : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                await Console.Out.WriteLineAsync($"The time is currently {DateTime.Now.ToString("h:mm:ss tt")}.");
            }
        }

    }
}
