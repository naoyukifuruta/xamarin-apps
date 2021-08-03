using System;
using Realms;

namespace SimpleTimecard.Models
{
    public class Timecard : RealmObject
    {
        [PrimaryKey]
        public string TimecardId { get; set; } = Guid.NewGuid().ToString();

        public DateTimeOffset? EntryDate { get; set; }

        public string StartTimeString { get; set; }

        public string EndTimeString { get; set; }
    }
}
