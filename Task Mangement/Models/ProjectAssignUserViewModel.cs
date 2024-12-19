namespace Task_Mangement.Models
{
    public class ProjectAssignUserViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; } // As project Id
        public List<int> UserId { get; set; }
    }
}
