using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class System
    {
        public Application application { get; set; }
        public User user { get; set; }

        public Device device { get; set; }
        public string apiEndpoint { get; set; }

        public string apiAccessToken { get; set; }
    }
}
