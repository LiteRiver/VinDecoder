using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinDecoder.Framework {
    public class Vehicle {
        public string Vin { get; set; }

        public string Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Series { get; set; }

        public string Style { get; set; }

        public override string ToString() {
            return string.Format("vin=[{0}], year=[{1}], make=[{2}], model=[{3}], series=[{4}], style=[{5}]", Vin, Year, Make, Model, Series, Style);
        }
    }
}
