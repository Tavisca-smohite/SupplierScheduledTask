using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tavisca.TravelNxt.Shared.Exceptions;

namespace Tavisca.SupplierScheduledTask.BusinessLogic.Exceptions
{
    [Serializable]
    public class DataException : BaseApplicationException
    {
        private const string CATEGORY = "DataException";

        public DataException()
            : base(CATEGORY)
        {
        }

        public DataException(string message)
            : base(CATEGORY, message)
        {
        }

        public DataException(string message, Exception innerException)
            : base(CATEGORY, message, innerException)
        {
        }

        public DataException(SerializationInfo info, StreamingContext context)
            : base(CATEGORY, info, context)
        {
        }
    }
}
