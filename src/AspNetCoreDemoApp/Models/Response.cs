using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class Response
    {
        public OutputSpeech outputSpeech { get; set; }
        public Card card { get; set; }

        public bool shouldEndSession { get; set; }
    }
}
