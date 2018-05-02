using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class Request
    {
        public string type { get; set; }
        public string requestId { get; set; }
        public string timestamp { get; set; }
        public string  locale { get; set; }
        public Intent intent { get; set; }
    }
}
