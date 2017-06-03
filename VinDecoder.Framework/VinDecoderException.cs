using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinDecoder.Framework {
    public class VinDecoderException : Exception {
        public VinDecoderException() : base() { }

        public VinDecoderException(string message) : base(message) { }

        public VinDecoderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
