//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Resources;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using Tavisca.SupplierScheduledTask.BusinessEntities;
//using Tavisca.SupplierScheduledTask.BusinessLogic.Contracts;
//using Tavisca.SupplierScheduledTask.Notifications;

//namespace Tavisca.SupplierScheduledTask.BusinessLogic.Controller
//{
//    public class ResourceDataController :IResourceDataController
//    {
//        public bool UpdateResourceFile(List<Supplier> disabledSuppliers )
//        {
//            var resourceEntries = ReadResourceFile();
//            //Modify resources here...
//            foreach (String key in data.Keys)
//            {
//                if (!resourceEntries.ContainsKey(key))
//                {
//                    String value = data[key].ToString();
//                    if (value == null)
//                        value = "";
//                    resourceEntries.Add(key, value);
//                }
//                else
//                {
//                    String value = data[key].ToString();
//                    if (value == null)
//                        value = "";
//                    resourceEntries.Remove(key);
//                    resourceEntries.Add(key, data[key].ToString());
//                }
//            }
//            //Write the combined resource file
//            ResXResourceWriter resourceWriter = new ResXResourceWriter(path);
//            foreach (String key in resourceEntries.Keys)
//            {
//                resourceWriter.AddResource(key, resourceEntries[key]);
//            }
//            resourceWriter.Generate();
//            resourceWriter.Close();
//            return false;
//        }

//        public Dictionary<string,string> ReadResourceFile()
//        {
//            var resourceEntries = new Dictionary<string,string>();
//            //Get existing resources
//            var reader = new ResXResourceReader(GetPath());
//            if (reader != null)
//            {
//                IDictionaryEnumerator id = reader.GetEnumerator();
//                foreach (DictionaryEntry d in reader)
//                {
//                    if (d.Value == null)
//                        resourceEntries.Add(d.Key.ToString(), "");
//                    else
//                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
//                } reader.Close();
//            }

//            return resourceEntries;
//        }

//        public bool DeleteFromResourceFile()
//        {
//            throw new NotImplementedException();
//        }

//        private string GetPath()
//        {
//            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
//            directoryPath = (directoryPath.EndsWith("\\bin\\Debug"))
//                                ? directoryPath.Replace("\\bin\\Debug", "")
//                                : directoryPath;

//            string path = directoryPath + Configuration.SuppliersConfigLogsFile;
//            return path;
//        }
//    }
//}
