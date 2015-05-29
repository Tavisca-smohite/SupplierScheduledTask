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
            
         var obj=new SupplierDataManagerDataContext().spGetLogBasedOnCallType2("HotelMultiAvail", 9, 1000, "Pegasus");
            //var data=SupplierData
            
        }
    }
}
