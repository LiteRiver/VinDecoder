using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace VinDecoder.Framework.Tests {
    [TestClass]
    public class VinCrawlerTest {
        [TestMethod]
        public void TestGetInitPage() {
            var crawler = new VinCrawler();
            var page = crawler.GetInitPage();

            Console.WriteLine(page);
        }

        [TestMethod]
        public void TestParseUrl() {
            var crawler = new VinCrawler();
            var url = crawler.ParseUrl();
            Console.WriteLine(url);
        }

        [TestMethod]
        public void TestQueryVin() {
            var crawler = new VinCrawler();
            var page = crawler.QueryVin("WDDGF5EBXBR135169");

            Console.WriteLine(page);
        }

        [TestMethod]
        public void TestParseVehicle() {
            var crawler = new VinCrawler();
            var vehicle = crawler.QueryVin("WDDGF5EBXBR135169");

            
        }
    }
}
