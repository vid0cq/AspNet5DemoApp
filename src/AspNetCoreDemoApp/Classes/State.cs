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

        public string TaxYear { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public decimal TaxDue { get; set; }

        #endregion

    }
}
