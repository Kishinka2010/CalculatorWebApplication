using System;
using System.Runtime.Serialization;

namespace СalculatorWebApplication.Exceptions

{
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }

    }
}