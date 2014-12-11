using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCRM.ViewModels
{
    public class ActivityListViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Start Date")]
        public string StartDateDisplay
        {
            get
            {
                if (StartDate == null)
                    return "";
                else
                    return string.Format("{0:yyyy-MM-dd hh:mm:ff tt}", StartDate);
            }
        }

        [Display(Name = "End Date")]
        public string EndDateDisplay
        {
            get
            {
                if (EndDate == null)
                    return "";
                else
                    return string.Format("{0:yyyy-MM-dd hh:mm:ff tt}", EndDate);
            }
        }

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

        public DateTime? StartDate { get; set; }
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
        
        public string Subject { get; set; }

        // used by full calendar
        public DateTime start
        {
            get
            {
                if (StartDate == null)
                    return DateTime.Parse("1900-01-01");
                else
                    return ((DateTime)StartDate).ToUniversalTime();
            }
        }

        public DateTime end
        {
            get
            {
                if (EndDate == null)
                    return DateTime.Parse("1900-01-01");
                else
                {
                    DateTime rtn = ((DateTime)EndDate).ToUniversalTime();
                    return rtn;
                }
            }
        }


        public WebCRM.Models.ActivityType? ActivityType { get; set; }
        public WebCRM.Models.ActivityStatus? ActivityStatus { get; set; }
    }
}