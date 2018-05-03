using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Models
{
    public class UpdatePDF
    {
        public string TaxYear { get; set; }
        public string email { get; set; }
        public string base64PDF { get; set; }
    }
}
