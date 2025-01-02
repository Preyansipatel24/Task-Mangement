namespace TaskManagementV1.Models.ResponseModels
{
    public class UserDetailResModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string EmployeeImage { get; set; }
        public string DepartmentName { get; set; }
        public int OrganizationId { get; set; }
        public string PrimaryColorCode { get; set; }
        public string OrganizationLogo { get; set; }
        public string OrganizationName { get; set; }
    }
}
