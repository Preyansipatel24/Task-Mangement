namespace Task_Mangement.Models
{
    public class ProjectAssignUserViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public List<int> UserId { get; set; }
    }
}
