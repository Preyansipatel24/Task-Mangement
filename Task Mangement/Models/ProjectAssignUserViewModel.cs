using System.ComponentModel.DataAnnotations;

namespace Task_Mangement.Models
{
    public class ProjectAssignUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project selection is required.")]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "User selection is required.")]
        public List<int> UserId { get; set; }
    }
}
