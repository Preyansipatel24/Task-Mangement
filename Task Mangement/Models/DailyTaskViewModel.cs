namespace Task_Mangement.Models
{
    public class DailyTaskViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime TaskDate { get; set; }
        public string TaskDuration { get; set; }
        public string TaskDescription { get; set; }
        public string TaskStatus { get; set; }
    }
}
