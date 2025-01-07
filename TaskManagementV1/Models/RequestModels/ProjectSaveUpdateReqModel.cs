namespace TaskManagementV1.Models.RequestModels
{
    public class ProjectSaveUpdateReqModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public int ReportingPersonUserId { get; set; }
    }
}
