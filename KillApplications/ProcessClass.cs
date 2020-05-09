namespace KillApplications
{
    public partial class KillApplicationsForm
    {
        public class ProcessClass
        {
            public ProcessClass(int id, string processName, string title, string applicationName, int counter)
            {
                Id = id;
                ProcessName = processName;
                Title = title;
                ApplicationName = applicationName;
                Kill = "Kill " + counter.ToString();
            }
            public int Id { get; set; }
            public string ProcessName { get; set; }
            public string Title { get; set; }
            public string ApplicationName { get; set; }
            public string Kill { get; set; }
        }
    }
}
