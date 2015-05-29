using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DBConfiguration
    {
        private static string ReadKayMatricesDatabaseKey
        {
            get { return GetKayMatricesDbConnectionString() ?? "LoggingDB"; }
        }

        private static string WriteKayMatricesDatabaseKey
        {
            get { return GetKayMatricesDbConnectionString() ?? "LoggingDB"; }
        }

        public static string ReadKayMatricesDatabaseConnection
        {
            get { return ReadKayMatricesDatabaseKey; }
        }

        public static string WriteKayMatricesDatabaseConnection
        {
            get { return WriteKayMatricesDatabaseKey; }
        }

        private static string GetKayMatricesDbConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["LoggingDB"].ConnectionString;
        }
    }
}
