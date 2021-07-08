using System;
namespace SimpleTimecard.Models
{
    public class Timecard
    {
        public string TimecardId { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }
    }
}
