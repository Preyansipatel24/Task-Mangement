namespace Task_Mangement.Models
{
    public class GetAllProjectAssignDetailList
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public List<UserDetail> UserDetailList { get; set; }
        public class UserDetail
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
        }
    }
}
