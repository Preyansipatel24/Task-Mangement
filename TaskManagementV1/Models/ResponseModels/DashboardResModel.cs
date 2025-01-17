namespace TaskManagementV1.Models.ResponseModels
{
    public class DashboardResModel
    {
        public List<GetTaskListResModel> TaskList { get; set; }
        public List<ProjectDetailResModel> ProjectList { get; set; }
    }
}
