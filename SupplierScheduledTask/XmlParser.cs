using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Xml.Linq;

using Entities;

namespace SupplierScheduledTask
{
    public class XmlParser
    {
       public List<Supplier> ParseSuppliers ()
       {
           string directoryPath = Directory.GetCurrentDirectory();
           directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
                               ? directoryPath.Replace("\\bin\\Debug", "")
                               : directoryPath;
           string path = Path.Combine(directoryPath, @"Suppliers.xml");
           var suppliersXml = XDocument.Load(path);
           
           List<XElement> suppliersList = suppliersXml.Descendants().Where(arg => arg.Name.LocalName == "Supplier").ToList();   

           return (from supplierNode in suppliersList
                   let id = supplierNode.Attribute("SupplierId")
                   let name = supplierNode.Attribute("Name")
                   let productType = supplierNode.Attribute("Type")
                   let callType = supplierNode.Element("CallType")
                   let threshholdValue = supplierNode.Element("ThreshholdValue")
                   select new Supplier
                       {
                           SupplierId = (id != null) ? Convert.ToInt32(id.Value) : 0, SupplierName = (name != null) ? name.Value : null, ProductType = (productType != null) ? productType.Value : null, CallType = (callType != null) ? callType.Value : null, ThreshholdValue = (threshholdValue != null) ? Convert.ToInt32(threshholdValue.Value) : 0
                       }).ToList();
            
        }
    }
}
