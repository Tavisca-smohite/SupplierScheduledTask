using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.Notifications;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
  
    public class SupplierDataHelper

{
    public List<Supplier> ParseSuppliers()
    {
        //string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
        //directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
        //                    ? directoryPath.Replace("\\bin\\Debug", "")
        //                    : directoryPath;

        string path = Configuration.SupplierData;
        var suppliersXml = XDocument.Load(path);

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
                        ThreshholdValue = (threshholdValue != null) ? float.Parse(threshholdValue.Value) : 0,
                        DisableIfCrossesThreshhold =
                            (shouldBeDisabled != null) ? Convert.ToInt32(shouldBeDisabled.Value) : 0
                    }).ToList();

    }

    public Dictionary<string, List<Supplier>> GetProductWiseSuppliersList()
    {
        var productWiseSuppliersList = new Dictionary<string, List<Supplier>>();
        //TODO: please refactor code by adding seperate private methods
        var suplierList = ParseSuppliers();
        WriteIntoLogFile("got parsed list of supliers...");
        var hotelSuppliersList =
            suplierList.FindAll(
                supplier =>
                string.Equals(supplier.ProductType, ProductTypes.ProductType.Hotel.ToString(),
                              StringComparison.InvariantCultureIgnoreCase));
        productWiseSuppliersList.Add(ProductTypes.ProductType.Hotel.ToString(), hotelSuppliersList);

        var airSuppliersList =
            suplierList.FindAll(
                supplier =>
                string.Equals(supplier.ProductType, ProductTypes.ProductType.Air.ToString(),
                              StringComparison.InvariantCultureIgnoreCase));
        productWiseSuppliersList.Add(ProductTypes.ProductType.Air.ToString(), airSuppliersList);

        var carSuppliersList =
            suplierList.FindAll(
                supplier =>
                string.Equals(supplier.ProductType, ProductTypes.ProductType.Car.ToString(),
                              StringComparison.InvariantCultureIgnoreCase));
        productWiseSuppliersList.Add(ProductTypes.ProductType.Car.ToString(), carSuppliersList);

        WriteIntoLogFile("returning dictinary to invoke- product wise supplier mapping");
        return productWiseSuppliersList;
    }

    public static Dictionary<Supplier, string> CompareThreshhold(Dictionary<Supplier, string> supplierAndFailureRateMapping)
    {
        var suppliersWhoCrossedThreshhold = new Dictionary<Supplier, string>();
        //TODO:use tryParse for conversion and use 'true' or false insted of '!'
        var threshholdForTotalCount = Convert.ToInt32(Configuration.ThreshholdForTotalCallsCount);
        foreach (var mapping in supplierAndFailureRateMapping)
        {
            Supplier supplier = mapping.Key;
            if(!string.Equals(mapping.Value,string.Empty))
            {
                var failureRate = float.Parse(mapping.Value);
                if (supplier.TotalCallsCount>=threshholdForTotalCount && supplier.ThreshholdValue <= failureRate)
                {
                    suppliersWhoCrossedThreshhold.Add(supplier, mapping.Value);
                }
            }
            else if(string.Equals(mapping.Value,string.Empty))
            {
                suppliersWhoCrossedThreshhold.Add(supplier, mapping.Value);
            }


        }
        return suppliersWhoCrossedThreshhold;
    }

        //TODO: move this method in seperate calss
        public static void WriteIntoLogFile(string data)
        {

            using (var file = new System.IO.StreamWriter(Configuration.LogFile, true))
            {
                file.WriteLine("-------------------------------------------------------");
                file.WriteLine(data);
            }            
        }

}

}

