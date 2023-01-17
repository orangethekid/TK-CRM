using System.Collections.Generic;

namespace TakraonlineCRM.Shared.User
{
    public class CurrentUser : User
    {
        public bool IsAuthenticated { get; set; }
        public Dictionary<string, string> Claims { get; set; }
    }
}
