using System;

namespace IxMilia.Iges
{
    public class IgesException : Exception
    {
        public IgesException()
            : base()
        {
        }

        public IgesException(string message)
            : base(message)
        {
        }

        public IgesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
