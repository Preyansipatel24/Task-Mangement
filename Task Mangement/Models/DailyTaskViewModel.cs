using System.ComponentModel.DataAnnotations;

namespace Task_Mangement.Models
{
    public class DailyTaskViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Project selection is required.")]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Task Date is required.")]
        public DateTime TaskDate { get; set; }
        [Required(ErrorMessage = "Task Duration is required.")]
        public string TaskDuration { get; set; }
        [Required(ErrorMessage = "Task Description is required.")]
        public string TaskDescription { get; set; }
        [Required(ErrorMessage = "TaskStatus selection is required.")]
        public string TaskStatus { get; set; }
    }
}
