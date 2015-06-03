using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;

using Entities;

namespace SupplierScheduledTask
{
    //TODO: Need to change class name and make this class as static also move methods in this class which are used commonly.
    public class XmlParser
    {
        public List<Supplier> ParseSuppliers()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
                                ? directoryPath.Replace("\\bin\\Debug", "")
                                : directoryPath;
            var path = HostingEnvironment.ApplicationPhysicalPath + "\\bin";
            string xmlFile = HostingEnvironment.MapPath("~/ApplicationData/MailBody.xml");
            
            //"\\ApplicationData";
           // var fullpath = path + "\ApplicationData\Supplier.xml";
            XDocument.Load(path);
            //string path = Path.Combine(directoryPath, @"Suppliers.xml");
            var suppliersXml = XDocument.Load(@".\ApplicationData\Supplier.xml");

            List<XElement> suppliersList =
                suppliersXml.Descendants().Where(arg => arg.Name.LocalName == "Supplier").ToList();

            return (from supplierNode in suppliersList
                    let id = supplierNode.Attribute("SupplierId")
                    let name = supplierNode.Attribute("Name")
                    let productType = supplierNode.Attribute("Type")
                    let callType = supplierNode.Element("CallType")
                    let threshholdValue = supplierNode.Element("ThreshholdValue")
                    let shouldBeDisabled = supplierNode.Element("DisableIfCrossesThreshhold")
                    select new Supplier
                        {
                            SupplierId = (id != null) ? Convert.ToInt32(id.Value) : 0,
                            SupplierName = (name != null) ? name.Value : null,
                            ProductType = (productType != null) ? productType.Value : null,
                            CallType = (callType != null) ? callType.Value : null,
                            ThreshholdValue = (threshholdValue != null) ? float.Parse(threshholdValue.Value) :0,
                            DisableIfCrossesThreshhold=(shouldBeDisabled!=null)?Convert.ToInt32(shouldBeDisabled.Value):0
                        }).ToList();

        }

        public Dictionary<string, List<Supplier>> GetProductWiseSuppliersList()
        {
            var productWiseSuppliersList = new Dictionary<string, List<Supplier>>();
                
            var suplierList = ParseSuppliers();      
                    
                    var hotelSuppliersList =
                        suplierList.FindAll(
                            supplier =>
                            string.Equals(supplier.ProductType,ProductTypes.ProductType.Hotel.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    productWiseSuppliersList.Add(ProductTypes.ProductType.Hotel.ToString(),hotelSuppliersList);

                    var airSuppliersList =
                               suplierList.FindAll(
                                   supplier =>
                                   string.Equals(supplier.ProductType, ProductTypes.ProductType.Air.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    productWiseSuppliersList.Add(ProductTypes.ProductType.Air.ToString(), airSuppliersList);

                    var carSuppliersList =
                                       suplierList.FindAll(
                                           supplier =>
                                           string.Equals(supplier.ProductType, ProductTypes.ProductType.Car.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    productWiseSuppliersList.Add(ProductTypes.ProductType.Car.ToString(), carSuppliersList);


            return productWiseSuppliersList;
        }             

}

}

