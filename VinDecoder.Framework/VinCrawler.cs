using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VinDecoder.Framework.Net;
using HtmlAgilityPack;

namespace VinDecoder.Framework {
    public class VinCrawler {
        private const string INIT_URL = "http://www.blackbookportals.com/bb/products/usedcarhtmlportal.asp";

        private CookieContainer m_cookies = new CookieContainer();
        private static readonly Regex s_actionRegex = new Regex("<form.+?action=\"(?<url>.+?)\">");

        public string GetInitPage() {
            var caller = new HttpCaller();
            var parameters = new HttpParameterCollection()
                .Add("color", "o")
                .Add("companyid", "thinkbank.COM");

            return caller.Get(INIT_URL, parameters, cookie: m_cookies);
        }

        public string ParseUrl() {
            var res = GetInitPage();

            var match = s_actionRegex.Match(res);
            return "http://www.blackbookportals.com/bb/products/" + match.Groups["url"].Value + "&vinchange=Y&showwholesale=True&showretail=True&showtradein=True";
        }

        public Vehicle QueryVin(string vin) {
            var url = ParseUrl();

            var caller = new HttpCaller();
            var parameters = new HttpParameterCollection()
                .Add("tvin", vin);

            var page =  caller.Post(url, parameters, cookie: m_cookies);

            var doc = new HtmlDocument();
            doc.LoadHtml(page);

            var vehicle = new Vehicle() {
                Vin = vin
            };

            var root = doc.DocumentNode;
            vehicle.Year = root.SelectNodes("//select[@id='bb_year']/option[@selected]").First().Attributes["value"].Value;
            vehicle.Make = root.SelectNodes("//select[@id='bb_make']/option[@selected]").First().Attributes["value"].Value;
            vehicle.Model = root.SelectNodes("//select[@id='bb_model']/option[@selected]").First().Attributes["value"].Value;
            vehicle.Series = root.SelectNodes("//select[@id='bb_Series']/option[@selected]").First().Attributes["value"].Value;
            vehicle.Style = root.SelectNodes("//select[@id='bb_bodystyle']/option[@selected]").First().Attributes["value"].Value;

            return vehicle;
        }
    }
}
