namespace TaskManagementV1.Models.RequestModels
{
    public class SaveUpdateTaskReqModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public DateTime? TaskDate { get; set; }
        public string TaskDuration { get; set; }
        public string TaskDescription { get; set; }
        public string TaskStatus { get; set; }
        public DateTime? TaskStartDateTime { get; set; }
        public DateTime? TaskEndDateTime { get; set; }
    }
}
