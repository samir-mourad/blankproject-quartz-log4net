using System;

namespace BlankProject.Infra.Quartz
{
    public class JobSchedule
    {
        public string Name { get; }
        public Type JobType { get; }

        public string CronExpression { get; }

        public JobSchedule(string name, Type jobType, string cronExpression)
        {
            Name = name;
            JobType = jobType;
            CronExpression = cronExpression;
        }
    }
}
