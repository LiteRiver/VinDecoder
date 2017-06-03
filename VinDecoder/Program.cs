using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinDecoder.Framework;

namespace VinDecoder {
    class Program {
        private static readonly ILog s_log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args) {

            s_log.Info("log4net initialized");
            
            Console.WriteLine("Input VIN to query vehicle information");
            Console.WriteLine("Input command 'exit' to exit program");

            var cmd = Console.ReadLine().Trim();
            var crawler = new VinCrawler();

            while (!"exit".Equals(cmd, StringComparison.InvariantCultureIgnoreCase)) {

                if (cmd.Length == 17) {
                    try {
                        Console.WriteLine("Retrieving...");
                        Console.WriteLine(crawler.QueryVin(cmd));
                    } catch {
                        Console.WriteLine("An error occured, get vehicle information failed.");
                    }
                                    
                } else {
                    Console.WriteLine("Not a valid vin number");
                }

                cmd = Console.ReadLine().Trim();
            }
        }
    }
}
