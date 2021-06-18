namespace BlankProject.Infra.Quartz
{
    public class JobConfig
    {
        public string Name { get; set; }

        public string TypeOf { get; set; }

        public string Cron { get; set; }

        public bool Disabled { get; set; }
    }
}
