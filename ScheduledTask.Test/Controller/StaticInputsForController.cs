using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.SupplierScheduledTask.BusinessEntities;

namespace ScheduledTask.Test
{
    public class StaticInputsForController
    {
        public static Dictionary<Supplier, string> DictionaryWithValidFailureRateAndTotalCallsCount(int no)
        {

            var dictionary = new Dictionary<Supplier, string>();
            switch (no)
            {
                case 1:
                    
                   dictionary= new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Hotel",
                            SupplierId=9,
                            TotalCallsCount=60,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=50,
                            IsDisabled=false},"70"}
                    };
                    break;
                case 2:
                    dictionary=
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=110,
                            TotalCallsCount=51,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=60,
                            IsDisabled=false},"75"}
                    };
                    break;
                case 3:
                    dictionary=
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            TotalCallsCount=50,
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
                            TotalCallsCount=49,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=50,
                            IsDisabled=false},"70"}
                    };
                    break;
                case 2:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=110,
                            TotalCallsCount=10,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=60,
                            IsDisabled=false},"75"}
                    };
                    break;
                case 3:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            TotalCallsCount=1,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=70,
                            IsDisabled=false},"70"}
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
                            TotalCallsCount=49,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=50,
                            IsDisabled=false},"10"}
                    };
                    break;
                case 2:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=110,
                            TotalCallsCount=100,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=60,
                            IsDisabled=false},"25"}
                    };
                    break;
                case 3:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            TotalCallsCount=1,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=70,
                            IsDisabled=false},"70"}
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
                            TotalCallsCount=49,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=50,
                            IsDisabled=false},"10"},

                    };
                    break;

                case 2:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Hotel",
                            SupplierId=117,
                            TotalCallsCount=0,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=60,
                            IsDisabled=false},""}
                    };
                    break;

                case 3:
                    dictionary =
              new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Air",
                            SupplierId=114,
                            TotalCallsCount=60,
                            DisableIfCrossesThreshhold=1,
                            ThreshholdValue=60,
                            IsDisabled=false},"60"}
                    };
                    break;
                case 4:
                    dictionary =
                    new Dictionary<Supplier, string>{
                        {new Supplier{ProductType="Car",
                            SupplierId=24,
                            TotalCallsCount=1,
                            DisableIfCrossesThreshhold=0,
                            ThreshholdValue=70,
                            IsDisabled=false},"70"}
                    };
                    break;
            }
            return dictionary;
        }
    }
}
