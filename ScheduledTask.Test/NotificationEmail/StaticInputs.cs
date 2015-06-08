using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace ScheduledTask.Test.NotificationEmail
{
   public class StaticInputs
    {
       public static Dictionary<Supplier, string> GetDictinoryWithBothTypesOfSuppliers()
       {
           var dictionary = new Dictionary<Supplier, string>()
                {
                    {
                        new Supplier()
                            {
                                SupplierId = 118,
                                SupplierName = "JacTravel",
                                IsDisabled = true,
                                ProductType = "Hotel",
                                DisableIfCrossesThreshhold = 1,
                                ThreshholdValue = 50
                            }, "50"
                    },
                    {
                        new Supplier()
                            {
                                SupplierId = 09,
                                SupplierName = "Pegasus",
                                IsDisabled = false,
                                ProductType = "Hotel",
                                DisableIfCrossesThreshhold = 1,
                                ThreshholdValue = 50
                            }, string.Empty
                    }
                };
           return dictionary;
       }

       public static Dictionary<Supplier, string> GetDictinoryWiththreshholdCrossedSuppliers()
       {
           var dictionary = new Dictionary<Supplier, string>()
                {
                    {
                        new Supplier()
                            {
                                SupplierId = 118,
                                SupplierName = "JacTravel",
                                IsDisabled = true,
                                ProductType = "Hotel",
                                DisableIfCrossesThreshhold = 1,
                                ThreshholdValue = 50
                            }, "50"
                    },
                    {
                        new Supplier()
                            {
                                SupplierId = 09,
                                SupplierName = "Pegasus",
                                IsDisabled = false,
                                ProductType = "Hotel",
                                DisableIfCrossesThreshhold = 1,
                                ThreshholdValue = 50
                            }, "60"
                    }
                };
           return dictionary;
       }

       public static Dictionary<Supplier, string> GetDictinorySuppliersWithInternalFailureWhileFetchingLogs()
       {
           var dictionary = new Dictionary<Supplier, string>()
                {
                    {
                        new Supplier()
                            {
                                SupplierId = 118,
                                SupplierName = "JacTravel",
                                IsDisabled = true,
                                ProductType = "Hotel",
                                DisableIfCrossesThreshhold = 0,
                                ThreshholdValue = 50
                            }, string.Empty
                    },
                    {
                        new Supplier()
                            {
                                SupplierId = 09,
                                SupplierName = "Pegasus",
                                IsDisabled = false,
                                ProductType = "Hotel",
                                DisableIfCrossesThreshhold = 1,
                                ThreshholdValue = 50
                            }, string.Empty
                    }
                };
           return dictionary;
       }
    }
}
