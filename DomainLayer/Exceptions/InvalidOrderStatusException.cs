using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class InvalidOrderStatusException : Exception
    {
        public string InvalidStatus { get; }

        public InvalidOrderStatusException(string invalidStatus)
            : base($"The order status '{invalidStatus}' is invalid")
        {
            InvalidStatus = invalidStatus;
        }
    }
}
