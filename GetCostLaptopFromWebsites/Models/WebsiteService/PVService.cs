using GetCostLaptopFromWebsites.Models.linq;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using vtctracking.Lib;

namespace GetCostLaptopFromWebsites.Models.WebsiteService
{
    public class PVService : IWebsiteService
    {
        const string tag = "PVService";
        public SourceWebsite _data { get; set; }
        public PVService(SourceWebsite W)
        {
            _data = W;
        }
        public async Task<bool> ProccessGetLaptopPrice()
        {
            try
            {
                LogAndConsole.LogInfo(tag, $"{_data.Website.WebSiteName} máy {_data.LaptopName}");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                //var dataBody = new System.Net.WebClient().DownloadString(_data.Link);
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = await web.LoadFromWebAsync(_data.Link);
                //HtmlNode node = doc.DocumentNode.SelectSingleNode("//body");
                var InnerHtml = doc.DocumentNode.SelectNodes("//*[@class=\"css-3725in\"]").FirstOrDefault().InnerHtml;
                //var lenInnerHtml = InnerHtml.Length;
                var pInnerHtmlTagFirst = InnerHtml.IndexOf("<");
                var price = int.Parse(InnerHtml.Substring(0, pInnerHtmlTagFirst).Replace(".", ""));
                LogAndConsole.LogInfo(tag, $"InnerHtml {InnerHtml}");

                try
                {
                    var db = new databaseDataContext();
                    var oldLaptopInfo = db.Laptop_infos.SingleOrDefault(x => x.SourceWebsiteId == _data.Id);
                    if (oldLaptopInfo == null)
                    {
                        var now = DateTime.Now;

                        var newLaptopInfo = new Laptop_info();
                        newLaptopInfo.SourceWebsiteId = _data.Id;
                        newLaptopInfo.Name = _data.LaptopName;
                        newLaptopInfo.Price = price;
                        newLaptopInfo.Created_date = now;

                        db.Laptop_infos.InsertOnSubmit(newLaptopInfo);
                        db.SubmitChanges();
                    }
                    else
                    {
                        if (oldLaptopInfo.Price != price)
                        {
                            oldLaptopInfo.Price = price;
                            oldLaptopInfo.Modified_date = DateTime.Now;
                            db.SubmitChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogAndConsole.LogError(tag, ex);
                    LogAndConsole.LogError(tag, "Lưu Laptop_info không thành công!");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogAndConsole.LogInfo(tag, ex);
                LogAndConsole.LogError(tag, "Có lỗi xử lý!");
                return false;
            }
        }
    }
}
