namespace Timesheet.Models
{
    public class Day
    {
        public int Category { get; set; }
        public string Date { get; set; }
        public string StartTime{ get; set; }
        public string EndTime{ get; set; }
        public string Commit{ get; set; }
        public string Description { get; set; }

        public Day ToFileObject()
        {
            return new Day()
            {
                Category = Category,
                Date = Date.Replace("/", ""),
                StartTime = StartTime.Replace(":", ""),
                EndTime = EndTime.Replace(":",""),
                Commit = string.IsNullOrEmpty(Commit) ? "" : Commit,
                Description = string.IsNullOrEmpty(Description) ? "" : Description
            };
        }
    }
}
