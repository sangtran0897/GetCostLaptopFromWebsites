using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCostLaptopFromWebsites.Models
{
    public interface IWebsiteService
    {
        Task<bool> ProccessGetLaptopPrice();
    }
}
