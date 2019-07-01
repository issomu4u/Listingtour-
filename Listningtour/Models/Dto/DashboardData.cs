using System.Collections.Generic;

namespace Listningtour.Models.Dto
{
    public class DashboardData
    {
        public DashboardData()
        {
            PublishedProperties = new List<PublishedProperty>();
        }
        public int Userinfoid { get; set; }

        public string Name { get; set; }
        
        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Zipcode { get; set; }

        public string Designation { get; set; }

        public string CellNo { get; set; }

        public string OfficeAddress { get; set; }

        public string Brokerage { get; set; }

        public string OfficeNo { get; set; }

        public string Website { get; set; }

        public string PhotoPath { get; set; }

        public string CompanyLogo { get; set; }
        
        public List<PublishedProperty> PublishedProperties { get; set; }
    }
}