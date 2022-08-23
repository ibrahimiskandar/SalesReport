using System;

namespace Reporter.Models
{
    public class ProcedureProps
    {
        public string ProcedureName { get; set; }
        public string Product { get; set; }
        public string Segment { get; set; }
        public string Country { get; set; }
        public DateTime SDATE { get; set; }
        public DateTime EDATE { get; set; }
    }
}
