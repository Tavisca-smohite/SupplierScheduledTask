using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Tavisca.SupplierScheduledTask.BusinessEntities;
using Tavisca.SupplierScheduledTask.Notifications;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
    public class ResourceDataController : IResourceDataController
    {
        public bool UpdateResourceFile(List<Supplier> disabledSuppliers)
        {
            var resourceEntries = ReadResourceFile();
            //Modify resources here...
            foreach (var disabledSupplier in disabledSuppliers)
            {
                //var key = disabledSupplier.SupplierId.ToString(CultureInfo.InvariantCulture);
                var key = GenerateKey(disabledSupplier.SupplierName, disabledSupplier.SupplierId.ToString(CultureInfo.InvariantCulture));
                var value = DateTime.Now.ToString(CultureInfo.InvariantCulture);  
                if (!resourceEntries.ContainsKey(key))
                {                                   
                    resourceEntries.Add(key, value);
                }
                else
                {                    
                    resourceEntries.Remove(key);
                    resourceEntries.Add(key, value);
                }
            }
            //Write the combined resource file
            var resourceWriter = new ResXResourceWriter(GetPath());
            foreach (var key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return false;
        }

        public Dictionary<string, string> ReadResourceFile()
        {
            var resourceEntries = new Dictionary<string, string>();
            //Get existing resources
            var reader = new ResXResourceReader(GetPath());
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                } reader.Close();
            }

            return resourceEntries;
        }

      //public bool RemoveEntriesFromResourceFile(List<Supplier> enabledSuppliers)
      // {
      //     var resourceEntries = ReadResourceFile();
      //     //Modify resources here...
      //     foreach (var enabledSupplier in enabledSuppliers)
      //     {
      //         //var key = enabledSupplier.SupplierId.ToString(CultureInfo.InvariantCulture);         
      //         var key = GenerateKey(enabledSupplier.SupplierName,enabledSupplier.SupplierId.ToString(CultureInfo.InvariantCulture));
      //         if (resourceEntries.ContainsKey(key))
      //         {
      //             resourceEntries.Remove(key);
      //         }              
      //     }
      //     //Write the combined resource file
      //     var resourceWriter = new ResXResourceWriter(GetPath());
      //     foreach (var key in resourceEntries.Keys)
      //     {
      //         resourceWriter.AddResource(key, resourceEntries[key]);
      //     }
      //     resourceWriter.Generate();
      //     resourceWriter.Close();
      //     return false;        
      // }

        public bool RemoveEntriesFromResourceFile(List<string> keys)
        {
            var resourceEntries = ReadResourceFile();
            //Modify resources here...
            foreach (var key in keys)
            {
                if (resourceEntries.ContainsKey(key))
                {
                    resourceEntries.Remove(key);
                }
            }
            //Write the combined resource file
            var resourceWriter = new ResXResourceWriter(GetPath());
            foreach (var key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
            return false;
        }

        public void RemoveAllEntriesInResourceFile()
        {
            var resourceEntries = ReadResourceFile();           
            resourceEntries.Clear();
            //Write the combined resource file
            var resourceWriter = new ResXResourceWriter(GetPath());           
            resourceWriter.Generate();
            resourceWriter.Close();
           
        }

        private string GetPath()
        {           
            string path = Configuration.SuppliersConfigLogsFile;
            return path;
        }

        private string GenerateKey(string name,string id)
        {
            var key = string.Format("{0}_{1}", name, id);                     
            return key;
        }      
    }
}
