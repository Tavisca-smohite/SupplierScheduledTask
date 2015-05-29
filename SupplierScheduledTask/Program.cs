using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;


namespace SupplierScheduledTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //var list = new XmlParser().ParseSuppliers();
            
         new SupplierDataManagerDataContext().spGetLogBasedOnCallType("HotelMultiAvail", 9, 1000, "Pegasus");
            //var data=SupplierData
        }
    }
}
