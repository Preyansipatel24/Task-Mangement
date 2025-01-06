namespace TaskManagementV1.Models.ResponseModels
{
    public class LoginResModel
    {
        public UserInfoDetail UserDetail { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public List<RoleAndPermissionDetailsList> roleAndPermissionList { get; set; }
    }

    public class RoleAndPermissionDetailsList
    {
        public int ModuleId { get; set; }
        public string ModuleDisplayName { get; set; }
        public string ModuleCode { get; set; }
        public bool IsClick { get; set; }
        public List<SubModuleDetailsList> SubModuleDetailsLists { get; set; }
    }

    public class SubModuleDetailsList
    {
        public int SubModuleId { get; set; }
        public string SubModuleDisplayName { get; set; }
        public string SubModuleCode { get; set; }
        public bool IsClick { get; set; }
        public List<ActionDetailsList> ActionLists { get; set; }
    }
    public class ActionDetailsList
    {
        public int ActionId { get; set; }
        public string ActionDisplayName { get; set; }
        public string ActionCode { get; set; }
        public bool IsClick { get; set; }
    }

    public class UserInfoDetail
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
