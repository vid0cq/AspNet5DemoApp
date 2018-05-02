using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class Context
    {
        public AudioPlayer AudioPlayer { get; set; }
        public Display Display;
        public System System { get; set; }
    }
}
