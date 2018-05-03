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

            XDocument xmlDoc = XDocument.Load(Path.Combine(Path.Combine(directory, "Files"),"State.xml"));

            XElement foundElement = FoundElement(xmlDoc, taxYear, email);

            if (foundElement != null)
            {
                taxState = new State();
                taxState.TaxYear = foundElement.Element("TaxYear").Value;
                taxState.Email = foundElement.Element("Email").Value;
                taxState.Status = foundElement.Element("Status").Value;
                taxState.TaxDue = decimal.Parse(foundElement.Element("TaxDue").Value);
                taxState.Name = foundElement.Element("Name").Value;
                taxState.IncomeSource = foundElement.Element("IncomeSource").Value;
            }

            return taxState;
        }

        public static object WriteState(string taxyear, string email, string status, decimal taxdue, string name, string incomeSource)
        {
            string dir = Environment.CurrentDirectory;
            XDocument xmlDoc = XDocument.Load(Path.Combine(Path.Combine(dir,"Files"),"State.xml"));

            XElement foundElement = FoundElement(xmlDoc, taxyear, email);

            if (foundElement != null)
            {
                foundElement.Element("Status").Value = status.ToString();
                foundElement.Element("TaxDue").Value = taxdue.ToString();
                foundElement.Element("Name").Value = name;
                foundElement.Element("IncomeSource").Value = incomeSource;
            }
            else
            {

                XElement root = new XElement("Client");
                root.Add(new XElement("TaxYear", taxyear));
                root.Add(new XElement("Email", email));
                root.Add(new XElement("Status", status));
                root.Add(new XElement("TaxDue", taxdue.ToString()));
                root.Add(new XElement("Name", name));
                root.Add(new XElement("IncomeSource", incomeSource));
                xmlDoc.Element("Clients").Add(root);
            }

            xmlDoc.Save(Path.Combine(Path.Combine(dir, "Files"), "State.xml"));
            return null;
    }

        private static XElement FoundElement(XDocument xmlDoc, string taxYear, string email)
        {
            var foundElement = xmlDoc.Root
                .Elements("Client")
                .Where(b => b.Element("TaxYear").Value == taxYear && b.Element("Email").Value == email)
                .FirstOrDefault();

            return foundElement;
        }

        public static bool SaveReturn(string taxReturnPath, byte[] bytePDF)
        {
            try
            {
                System.IO.File.WriteAllBytes(taxReturnPath+".pdf", bytePDF);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public static bool ReturnExists(string taxReturnPath)
        {
            if (File.Exists(taxReturnPath))
                return true;
            else return false;
        }
    }
}
