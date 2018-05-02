using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class Device
    {
        public string deviceId { get; set; }

        public SupportedInterfaces supportedInterfaces { get; set; }
    }
}
