using HtmlAgilityPack;
using log4net;
using System;
using System.Linq;
using System.Net;
using VinDecoder.Framework.Net;

namespace VinDecoder.Framework {
    public class VinCrawler {
        private const string INIT_URL = "http://www.blackbookportals.com/bb/products/usedcarhtmlportal.asp";

        private CookieContainer m_cookies = new CookieContainer();

        private static readonly ILog s_logger = LogManager.GetLogger(typeof(VinCrawler));

        public string GetInitPage() {
            var caller = new HttpCaller();
            var parameters = new HttpParameterCollection()
                .Add("color", "o")
                .Add("companyid", "thinkbank.COM");

            return caller.Get(INIT_URL, parameters, cookie: m_cookies);
        }

        public string ParseUrl() {
            s_logger.Info("starting to retrieve initialization page");
            var page = GetInitPage();
            s_logger.Info("Initialization page retrieved");
            s_logger.Info(page);

            var doc = new HtmlDocument();
            doc.LoadHtml(page);

            var action = doc.DocumentNode.SelectSingleNode("//form").Attributes["action"].Value;
            action = "http://www.blackbookportals.com/bb/products/" + action + "&vinchange=Y&showwholesale=True&showretail=True&showtradein=True";

            s_logger.InfoFormat("action url parsed => [{0}]", action);
            return action;
        }

        public Vehicle QueryVin(string vin) {
            try {
                var url = ParseUrl();

                var caller = new HttpCaller();
                var parameters = new HttpParameterCollection()
                    .Add("tvin", vin);

                s_logger.Info("starting to retrieve vehicle page");
                var page = caller.Post(url, parameters, cookie: m_cookies);
                s_logger.Info("vehicle page retrieved");
                s_logger.Info(page);

                var doc = new HtmlDocument();
                doc.LoadHtml(page);

                var vehicle = new Vehicle() {
                    Vin = vin
                };

                var root = doc.DocumentNode;
                vehicle.Year = root.SelectSingleNode("//select[@id='bb_year']/option[@selected]").Attributes["value"].Value;
                vehicle.Make = root.SelectSingleNode("//select[@id='bb_make']/option[@selected]").Attributes["value"].Value;
                vehicle.Model = root.SelectSingleNode("//select[@id='bb_model']/option[@selected]").Attributes["value"].Value;
                vehicle.Series = root.SelectSingleNode("//select[@id='bb_Series']/option[@selected]").Attributes["value"].Value;
                vehicle.Style = root.SelectSingleNode("//select[@id='bb_bodystyle']/option[@selected]").Attributes["value"].Value;                

                return vehicle;

            } catch(Exception ex) {
                s_logger.Error("An error Occured", ex);
                throw new VinDecoderException("An error Occured", ex);
            }

        }
    }
}
