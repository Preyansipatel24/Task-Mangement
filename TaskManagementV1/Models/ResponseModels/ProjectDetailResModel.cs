namespace TaskManagementV1.Models.ResponseModels
{
    public class ProjectDetailResModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public string ProjectStartDateStr { get; set; }
        public string ProjectEndDateStr { get; set; }
    }
}
