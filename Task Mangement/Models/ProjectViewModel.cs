using System.ComponentModel.DataAnnotations;

namespace Task_Mangement.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Project Status selection is required.")]
        public string ProjectStatus { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
    }
}
