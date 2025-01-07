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
        public int ReportingPersonUserId { get; set; }
        public string ReportingPersonFullName { get; set; }
        public string ReportingPersonEmail { get; set; }
        public int ReportingPersonRoleId { get; set; }
        public string ReportingPersonRoleName { get; set; }
        public int ReportingPersonDepartmentId { get; set; }
        public string ReportingPersonDepartmentName { get; set; }
        public int ReportingPersonDesignationId { get; set; }
        public string ReportingPersonDesignationName { get; set; }
    }
}
