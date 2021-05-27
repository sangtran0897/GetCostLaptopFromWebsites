using GetCostLaptopFromWebsites.Models.linq;
using GetCostLaptopFromWebsites.Models.WebsiteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCostLaptopFromWebsites.Models
{
    public class WebsiteFactory
    {
        public static IWebsiteService GetAgency(SourceWebsite SourceWebsite)
        {
            if(SourceWebsite.Website.WebSiteName == "Phong Vũ")
            {
                return new PVService(SourceWebsite);
            }
            else if (SourceWebsite.Website.WebSiteName == "Laptop 123")
            {
                return new L123Service(SourceWebsite);
            }
            else if (SourceWebsite.Website.WebSiteName == "ThinkPro")
            {
                return new PVService(SourceWebsite);
            }
            else if (SourceWebsite.Website.WebSiteName == "FPT Soft")
            {
                return new PVService(SourceWebsite);
            }
            else if (SourceWebsite.Website.WebSiteName == "Điện máy xanh")
            {
                return new PVService(SourceWebsite);
            }
            else if (SourceWebsite.Website.WebSiteName == "Nguyễn Kim")
            {
                return new PVService(SourceWebsite);
            }
            return new PVService(SourceWebsite);
        }
    }
}
