using System;

namespace TakraonlineCRM.Shared.Models
{
    public class ActivitiesLog : BaseEntity
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string UserRole { get; set; }
        public string PageAction { get; set; }
        public string Actionlog { get; set; }
        public string BackupObject { get; set; }
    }
}
