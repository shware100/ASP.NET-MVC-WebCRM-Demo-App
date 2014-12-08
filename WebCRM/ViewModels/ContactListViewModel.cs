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
        [Display(Name = "Professional Title")]
        public String Title { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }

        [Display(Name = "Company Name")]
        public String CompanyName { get; set; }

        public string Name
        {
            get
            {
                return FullName;
            }
        }

        public String Honorific { get; set; }
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public String MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        public String Suffix { get; set; }

    }
}