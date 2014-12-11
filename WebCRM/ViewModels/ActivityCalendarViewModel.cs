using System;
using System.ComponentModel.DataAnnotations;

namespace WebCRM.ViewModels
{
    public class ActivityCalendarViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Activity Type")]
        public string ActivityTypeDisplay
        {
            get
            {
                return ActivityType.ToString();
            }
        }

        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        public DateTime? start { get; set; }
        public DateTime? EndDate { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Activity Status")]
        public string ActivityStatusDisplay
        {
            get
            {
                return ActivityStatus.ToString();
            }
        }

        public DateTime? end { get { return EndDate; } }
        public string title
        {
            get
            {
                return string.Format("{0} {1}", ActivityTypeDisplay, ContactName);
            }
        }

        public WebCRM.Models.ActivityType? ActivityType { get; set; }
        public WebCRM.Models.ActivityStatus? ActivityStatus { get; set; }
 
    }
}