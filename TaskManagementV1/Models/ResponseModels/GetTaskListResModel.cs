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
        public DateTime? TaskStartDateTime { get; set; }
        public DateTime? TaskEndDateTime { get; set; }
        public string TaskStartDateTimeStr { get; set; }
        public string TaskEndDateTimeStr { get; set; }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        public string ProjectStatus { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public int ReportingPersonUserId { get; set; }
        public string ReportingPersonFullName { get; set; }
        public string ReportingPersonEmail { get; set; }
        public string ProjectStartDateStr { get; set; }
        public string ProjectEndDateStr { get; set; }
    }
}
