using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCRM.Models
{

    // TODO: move these to a enum / lookup table
    public enum ActivityType
    {
        Call,
        Document,
        Email,
        Letter,
        Meeting,
        Task
    }

    public enum ActivityStatus
    {
        Active,
        Canceled,
        Completed,
        Pending

    }
    public class ActivityHistory
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Activity Type")]
        public ActivityType? ActivityType { get; set; }

        [Required]
        [Display(Name = "Activity Status")]
        public ActivityStatus? ActivityStatus { get; set; }

        public int CompanyID { get; set; }
        public int ContactID { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Comments { get; set; }
        
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Created By")]
        public int CreatedByID { get; set; }
        [Display(Name = "Updated At")]
        public DateTime Updatedat { get; set; }
        [Display(Name = "Updated By")]
        public int UpdatedByID { get; set; }

        public virtual Company Company {get; set;}
        public virtual Contact Contact {get; set;}
    }
}