using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCRM.ViewModels
{
    public class CompanyListViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string CityRegion { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        // public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}