namespace TaskManagementV1.Models.ResponseModels
{
    public class GetTaskListResModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime TaskDate { get; set; }
        public string TaskDateStr { get; set; }
        public string TaskDuration { get; set; }
        public string TaskDescription { get; set; }
        public string TaskStatus { get; set; }
    }
}
