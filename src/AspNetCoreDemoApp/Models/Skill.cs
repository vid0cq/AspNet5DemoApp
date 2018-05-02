using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class Skill
    {
        public string version { get; set; }

        public Session session { get; set; }

        public Context context { get; set; }

        public Request request { get; set; }

        public Response response { get; set; }

        public Dictionary<string, string> sessionAttributes { get; set; }
    }
}
