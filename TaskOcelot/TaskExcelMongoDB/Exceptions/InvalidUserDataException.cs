using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskExcelMongoDB.Exceptions
{
    public class InvalidUserDataException : Exception
    {
        public InvalidUserDataException(string message) : base(message) { }
    }
}