using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCRM.Models
{
    public class Contact
    {
        public int ID { get; set; }
        public string Honorific { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        [Display(Name = "Alt. Email")]
        public string AlternateEmail { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Created By")]
        public int CreatedByID { get; set; }
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
        [Display(Name = "Updated By")]
        public int UpdatedByID { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<ActivityHistory> ActivityHistory { get; set; }

    }
}