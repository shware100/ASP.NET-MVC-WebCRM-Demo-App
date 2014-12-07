using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCRM.ViewModels
{
    public class ContactListViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                string s = "";
                
                if (Honorific != null)
                    s += Honorific + " ";
                s += FirstName + " ";
                if (MiddleName != null)
                {
                    if (MiddleName.Length == 1)
                        s += MiddleName + ". ";
                    else
                        s += MiddleName + " ";
                }
                s += LastName;
                if (Suffix != null)
                    s += ", " + Suffix;
                return s;
            }
        }
        public string Name
        {
            get
            {
                return FullName;
            }
        }
        public string Honorific { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Suffix { get; set; }

        [Display(Name = "Professional Title")]
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

    }
}