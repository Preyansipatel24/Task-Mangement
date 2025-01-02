namespace TaskManagementV1.Models.RequestModels
{
    public class SaveUpdateProjectAssignmentReqModel
    {
        public int ProjectId { get; set; }
        public List<int> UserIdList { get; set; }
    }
}
