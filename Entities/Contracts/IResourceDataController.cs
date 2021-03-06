﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace Tavisca.SupplierScheduledTask.BusinessLogic
{
    public interface IResourceDataController
    {
        bool UpdateResourceFile(List<Supplier> disabledSuppliers);
        bool AddResourcesToFile(Dictionary<string,string> data);
        Dictionary<string, string> ReadResourceFile();
        //bool RemoveEntriesFromResourceFile(List<Supplier> disabledSuppliers);
        bool RemoveEntriesFromResourceFile(List<String> keys);
        void RemoveAllEntriesInResourceFile();
    }
}
