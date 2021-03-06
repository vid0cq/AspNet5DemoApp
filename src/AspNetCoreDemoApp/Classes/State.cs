using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemoApp.Classes
{
    public class State
    {
        #region Constructors
        public State()
        {

        }

        #endregion

        #region Properties

        [JsonProperty("TaxYear")]
        public string TaxYear { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Status")]
        public string Status { get; set; }
        [JsonProperty("TaxDue ")]
        public decimal TaxDue { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("IncomeSource")]
        public string IncomeSource { get; set; }

        #endregion

    }
}
