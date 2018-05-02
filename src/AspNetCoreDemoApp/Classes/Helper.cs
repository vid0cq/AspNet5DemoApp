using System;
using System.Collections.Generic;
using System.IO;
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
        public static State ReadState(string taxYear, string email)
        {
            State taxState = null;

            string directory = Environment.CurrentDirectory;

            XDocument xmlDoc = XDocument.Load(directory + "\\Files\\State.xml");

            XElement foundElement = FoundElement(xmlDoc, taxYear, email);

            if (foundElement != null)
            {
                taxState = new State();
                taxState.TaxYear = foundElement.Element("TaxYear").Value;
                taxState.Email = foundElement.Element("Email").Value;
                taxState.Status = int.Parse(foundElement.Element("Status").Value);
                taxState.TaxDue = decimal.Parse(foundElement.Element("TaxDue").Value);
            }

            return taxState;
        }

        public static object WriteState(string taxyear, string email, int status, decimal taxdue)
        {
            try
            {
                string dir = Environment.CurrentDirectory;
                if (!File.Exists(Path.Combine(dir,"State.xml")))
                {   
                    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AspNetCoreDemoApp.Files.State.xml");
                    StreamWriter sw = new StreamWriter(Path.Combine(dir, "State.xml"));
                    sw.Write(stream);
                    sw.Close();
                }

                XDocument xmlDoc = XDocument.Load(Path.Combine(dir, "State.xml"));

                XElement foundElement = FoundElement(xmlDoc, taxyear, email);

                if (foundElement != null)
                {
                    foundElement.Element("Status").Value = status.ToString();
                    foundElement.Element("TaxDue").Value = taxdue.ToString();
                }
                else
                {

                    XElement root = new XElement("Client");
                    root.Add(new XElement("TaxYear", taxyear));
                    root.Add(new XElement("Email", email));
                    root.Add(new XElement("Status", status.ToString()));
                    root.Add(new XElement("TaxDue", taxdue.ToString()));
                    xmlDoc.Element("Clients").Add(root);
                }

                xmlDoc.Save(Path.Combine(dir, "State.xml"));
                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }

        private static XElement FoundElement(XDocument xmlDoc, string taxYear, string email)
        {
            var foundElement = xmlDoc.Root
                .Elements("Client")
                .Where(b => b.Element("TaxYear").Value == taxYear && b.Element("Email").Value == email)
                .FirstOrDefault();

            return foundElement;
        }
    }
}
