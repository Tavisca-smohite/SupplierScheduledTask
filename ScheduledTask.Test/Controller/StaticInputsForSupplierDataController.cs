using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace ScheduledTask.Test
{
    public class StaticInputsForSupplierDataController
    {
        public static Dictionary<Supplier, string> DictionaryWithValidFailureRateAndTotalCallsCount(int no)
        {

            var dictionary = new Dictionary<Supplier, string>();
            switch (no)
            {
                case 1:
                    
                   dictionary= new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Hotel",
                            SupplierName="Pegasus",
                            SupplierId=9,
                            TotalCallsCount=60,
                            TotalSuccessfulCallsCount= 18,
                            TotalFailureCallsCount= 42,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=50,
                            IsDisabled=false},"70"}
                    };
                    break;
                case 2:
                    dictionary=
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=110,
                            SupplierName="Mystifly",
                            TotalCallsCount=51,
                            TotalSuccessfulCallsCount= 13,
                            TotalFailureCallsCount= 38,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=60,
                            IsDisabled=false},"75"}
                    };
                    break;
                case 3:
                    dictionary=
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            SupplierName="SabreCar",
                            TotalCallsCount=50,
                            TotalSuccessfulCallsCount= 15,
                            TotalFailureCallsCount= 35,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=70,
                            IsDisabled=false},"70"}
                    };
                    break;
            }
            return dictionary;
        }

        public static Dictionary<Supplier, string> DictionaryWithValidFailureRateAndInvalidTotalCallsCount(int no)
        {

            var dictionary = new Dictionary<Supplier, string>();
            switch (no)
            {
                case 1:

                    dictionary = new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Hotel",
                            SupplierId=9,
                            SupplierName="Pegasus",
                            TotalCallsCount=49,
                            TotalSuccessfulCallsCount= 15,
                            TotalFailureCallsCount= 34,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=50,
                            IsDisabled=false},"70"}
                    };
                    break;
                case 2:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=110,
                            SupplierName="Mystifly",
                            TotalCallsCount=10,
                            TotalSuccessfulCallsCount= 3,
                            TotalFailureCallsCount= 7,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=60,
                            IsDisabled=false},"75"}
                    };
                    break;
                case 3:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            SupplierName="SabreCar",
                            TotalCallsCount=1,
                            TotalSuccessfulCallsCount = 0,
                            TotalFailureCallsCount = 1,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=70,
                            IsDisabled=false},"100"}
                    };
                    break;
            }
            return dictionary;
        }

        public static Dictionary<Supplier, string> DictionaryWithValidFailureRateOrInvalidTotalCallsCount(int no)
        {

            var dictionary = new Dictionary<Supplier, string>();
            switch (no)
            {
                case 1:

                    dictionary = new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Hotel",
                            SupplierId=9,
                            SupplierName="Pegasus",
                            TotalCallsCount=49,
                            TotalSuccessfulCallsCount = 44,
                            TotalFailureCallsCount = 5,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=50,
                            IsDisabled=false},"10"}
                    };
                    break;
                case 2:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=110,
                            SupplierName="Mystifly",
                            TotalCallsCount=100,
                            TotalSuccessfulCallsCount = 75,
                            TotalFailureCallsCount = 25,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=60,
                            IsDisabled=false},"25"}
                    };
                    break;
                case 3:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            SupplierName="SabreCar",
                            TotalCallsCount=1,
                            TotalSuccessfulCallsCount = 0,
                            TotalFailureCallsCount = 1,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=70,
                            IsDisabled=false},"100"}
                    };
                    break;
            }
            return dictionary;
        }

        public static Dictionary<Supplier, string> DictionaryWithValidAsWellAsInValidFailureRateOrTotalCallsCount(int no)
        {

            var dictionary = new Dictionary<Supplier, string>();
            switch (no)
            {
                case 1:

                    dictionary = new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Hotel",
                            SupplierId=9,
                            SupplierName="Pegasus",
                            TotalCallsCount=49,
                            TotalSuccessfulCallsCount = 44,
                            TotalFailureCallsCount = 5,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=50,
                            IsDisabled=false},"10"},

                    };
                    break;

                case 2:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Hotel",
                            SupplierId=117,
                            SupplierName="PricelineV3",
                            TotalCallsCount=0,
                            TotalSuccessfulCallsCount = 0,
                            TotalFailureCallsCount = 0,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=60,
                            IsDisabled=false},""}
                    };
                    break;

                case 3:
                    dictionary =
              new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=16,
                            SupplierName="AmadeusAir",
                            TotalCallsCount=60,
                            TotalSuccessfulCallsCount = 24,
                            TotalFailureCallsCount = 36,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=60,
                            IsDisabled=false},"60"}
                    };
                    break;
                case 4:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            SupplierName="SabreCar",
                            TotalCallsCount=1,
                            TotalSuccessfulCallsCount = 0,
                            TotalFailureCallsCount = 1,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=70,
                            IsDisabled=false},"100"}
                    };
                    break;
            }
            return dictionary;
        }
    }
}
