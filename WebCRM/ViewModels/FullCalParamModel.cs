using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCRM.ViewModels
{
    public class FullCalParamModel
    {
        public string start { get; set; }
        public string end { get; set; }

        public DateTime? StartDate
        {
            get
            {
                if (string.IsNullOrEmpty(start))
                    return null;
                else
                    return DateTime.Parse(start);
            }
        }
        public DateTime? EndDate
        {
            get
            {
                if (string.IsNullOrEmpty(end))
                    return null;
                else
                    return DateTime.Parse(end);
            }
        }

    }
}