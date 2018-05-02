using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AspNetCoreDemoApp.Classes
{
    public static class Helper
    {
        public static State ReadState()
        {
            State taxState = new State();

            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Files.State.xml"))
            {
                XDocument xmlDoc = XDocument.Load(stream);
                taxState.TaxYear = xmlDoc.Root.Element("TaxYear").Value;
                taxState.Email = xmlDoc.Root.Element("Email").Value;
                if (string.IsNullOrEmpty(xmlDoc.Root.Element("Status").Value))
                    taxState.Status = 0;
                else taxState.Status = int.Parse(xmlDoc.Root.Element("Status").Value);

                if (string.IsNullOrEmpty(xmlDoc.Root.Element("TaxDue").Value))
                    taxState.TaxDue = 0;
                else taxState.TaxDue = decimal.Parse(xmlDoc.Root.Element("TaxDue").Value);
            }

            return taxState;
        }

        public static void WriteState(string taxyear, string email, int status, decimal taxdue)
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream("Files.State.xml"))
            {
                XDocument xmlDoc = XDocument.Load(stream);
                xmlDoc.Root.Element("TaxYear").Value = taxyear;
                xmlDoc.Root.Element("Email").Value = email;
                xmlDoc.Root.Element("Status").Value = status.ToString();
                xmlDoc.Root.Element("TaxDue").Value = taxdue.ToString();

                xmlDoc.Save(stream);
            }
        }
    }
}
