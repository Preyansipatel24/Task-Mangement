namespace Task_Mangement.Models
{
    public class DownloadReportViewModel
    {
        public bool IsUserWiseReport { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
    }
}
