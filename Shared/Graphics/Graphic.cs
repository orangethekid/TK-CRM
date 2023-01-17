using System;

namespace TakraonlineCRM.Shared.Graphics
{
    public class Graphic : BaseEntity
    {
        public Graphic()
        {
            DraftDate = DateTime.Now;
        }

        public int OrderId { get; set; }
        //
        public DateTime DraftDate { get; set; }
        public string FeedBack { get; set; }
        public string DraftFile { get; set; }
    }
}
