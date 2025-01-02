namespace TaskManagementV1.Models.ResponseModels
{
    public class ProjectUserListResModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public List<ProjectUserDetail> ProjectUserDetailList { get; set; }
    }

    public class ProjectUserDetail
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
