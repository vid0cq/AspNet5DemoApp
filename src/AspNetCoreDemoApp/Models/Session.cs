using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class Session
    {
        public Boolean niu { get; set; }
        public string sessionId { get; set; }
        public Application application { get; set; }
        public User user { get; set; }
    }
}
