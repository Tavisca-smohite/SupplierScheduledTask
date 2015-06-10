using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.SupplierScheduledTask.DataAccessLayer
{
    public class DBConfiguration
    {
        private static string ReadSupplierDataDatabaseKey
        {
            get { return GetSupplierDataDbConnectionString() ?? "LoggingDB"; }
        }

        private static string WriteSupplierDataDatabaseKey
        {
            get { return GetSupplierDataDbConnectionString() ?? "LoggingDB"; }
        }

        public static string ReadSupplierDataDatabaseConnection
        {
            get { return ReadSupplierDataDatabaseKey; }
        }

        public static string WriteSupplierDataDatabaseConnection
        {
            get { return WriteSupplierDataDatabaseKey; }
        }

        private static string GetSupplierDataDbConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["LoggingDB"].ConnectionString;
        }

        private static string ReadSupplierConfigDatabaseKey
        {
            get { return GetSupplierConfigDbConnectionString() ?? "CommonDB"; }
        }

        private static string WriteSupplierConfigDatabaseKey
        {
            get { return GetSupplierConfigDbConnectionString() ?? "CommonDB"; }
        }

        public static string ReadSupplierConfigDatabaseConnection
        {
            get { return ReadSupplierConfigDatabaseKey; }
        }

        public static string WriteSupplierConfigDatabaseConnection
        {
            get { return WriteSupplierConfigDatabaseKey; }
        }

        private static string GetSupplierConfigDbConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["CommonDB"].ConnectionString;
        }
    }
}
