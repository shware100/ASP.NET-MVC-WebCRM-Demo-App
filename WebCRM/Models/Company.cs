namespace WebCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = "City / Region")]
        public string CityRegion { get; set; }
        [Display(Name = "State / Province")]
        public string StateProvince { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Created By")]
        public int CreatedByID { get; set; }
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
        [Display(Name = "Updated By")]
        public int UpdatedByID { get; set; }

        public virtual ICollection<Contact> Contacts {get; set;}
        public virtual ICollection<ActivityHistory> ActivityHistory { get; set; }

    }
}